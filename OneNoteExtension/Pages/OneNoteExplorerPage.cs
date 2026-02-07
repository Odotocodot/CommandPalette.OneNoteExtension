using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerPage : SearchPage
{
    private readonly IOneNoteItem _item;
    private readonly Lock _searchUpdateLock = new();

    private SearchParameters _searchParameters;

    private static readonly SearchParameters _scopeSearch = new(
        (search, page) => OneNoteHelper.FindPages(search, page._item).AsListItems(true, true),
        (search, page) => page.OnSearchChanged(search, true));

    private static readonly SearchParameters _titleSearch = new(
        (search, page) => page._item.Descendants().FilterItems(search).AsListItems(true, true),
        (search, page) => page.OnSearchChanged(search, false));

    private static readonly SearchParameters _childrenSearch = new(
        (search, page) => string.IsNullOrWhiteSpace(search)
            ? page._item.Children.AsListItems(false, false).Prepend(new OpenOrCreateItemListItem(page._item))
            : page._item.Children.FilterItems(search).AsListItems(false, false),
        (search, page) =>
        {
            page._searchItems.Clear();
            page.GetMoreItems(search);
            page.EmptyContent = page._searchItems.Count == 0 ? EmptyContentHelper.GetNotMatchesFoundWithCommands(page._item) : null;
        });

    public OneNoteExplorerPage(IOneNoteItem item)
    {
        _item = item;
        if (item is not Section)
        {
            var filters = new OneNoteExplorerFilters();
            filters.PropChanged += Filters_PropChanged;
            Filters = filters;
        }
        var relativePath = PageHelper.GetSubtitle(item, true);
        Title = item is Notebook notebook
            ? $"{Resources.OneNoteExplorer} | {notebook.DisplayName}"
            : $"{Resources.OneNoteExplorer} | {relativePath}";
        Icon = Icons.GetIcon(item);
        Name = Resources.Enter;
        HasMoreItems = true;
        _searchParameters = _childrenSearch;
        PageLoaded += () => UpdateSearchText(string.Empty, SearchText);
    }

    private void Filters_PropChanged(object sender, IPropChangedEventArgs args)
    {
        _searchParameters = Filters?.CurrentFilterId switch
        {
            OneNoteExplorerFilters.ScopeSearchFilterId => _scopeSearch,
            OneNoteExplorerFilters.TitleSearchFilterId => _titleSearch,
            _ => _childrenSearch,
        };
        UpdateSearchText(string.Empty, SearchText);
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        lock (_searchUpdateLock) //Due to UpdateSearchText being called with PageLoaded, sometimes items are duplicated in the search
        {
            _searchParameters.OnSearchChanged(newSearch, this);
        }
        RaiseItemsChanged(_searchItems.Count);
    }

    protected override IEnumerable<ListItem> GetItemsAction(string search) => _searchParameters.GetItemsAction(search, this);

    private record SearchParameters(Func<string, OneNoteExplorerPage, IEnumerable<ListItem>> GetItemsAction, Action<string, OneNoteExplorerPage> OnSearchChanged);

    private partial class OneNoteExplorerFilters : Filters
    {
        public const string ChildrenFilterId = "default";
        public const string ScopeSearchFilterId = "scope";
        public const string TitleSearchFilterId = "title";
        public override IFilterItem[] GetFilters() =>
        [
            new Filter { Id = ChildrenFilterId, Name = Resources.DefaultFilter},// Viewing direct children
            new Filter { Id = ScopeSearchFilterId, Name = Resources.Pages, Icon = Icons.Page },
            new Filter { Id = TitleSearchFilterId, Name = Resources.Titles, Icon = Icons.Title },
        ];
    }
}
