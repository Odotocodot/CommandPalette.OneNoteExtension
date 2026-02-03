using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;
using System.Collections.Generic;
using System.Linq;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerPage : DynamicListPageExt
{
    private readonly IOneNoteItem _item;
    private readonly int _resultsPerLoad = 25;
    private readonly List<ListItem> _searchItems = [];

    public OneNoteExplorerPage(IOneNoteItem item)
    {
        _item = item;
        if(item is not Section)
        {
            var filters = new OneNoteExplorerFilters();
            filters.PropChanged += (_, _) => UpdateSearchText(string.Empty, string.Empty);
            Filters = filters;
        }
        var relativePath = OneNoteHelper.GetSubtitle(item, true);
        Title = item is Notebook notebook
            ? $"{Resources.OneNoteExplorer} | {notebook.DisplayName}"
            : $"{Resources.OneNoteExplorer} | {relativePath}";
        Icon = Icons.GetIcon(item);
        Name = Resources.Enter;
        EmptyContent = EmptyContentHelper.NoChildren;
        PageLoaded += () =>
        {
            UpdateSearchText(string.Empty, string.Empty);
        };
    }

    public override IListItem[] GetItems() => [.. _searchItems];

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        switch(Filters?.CurrentFilterId)
        {
            case OneNoteExplorerFilters.ScopeSearchFilterId:
                _searchItems.Clear();
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    EmptyContent = EmptyContentHelper.EmptySearch;
                    break;
                }

                if (!char.IsLetterOrDigit(SearchText[0]))
                {
                    EmptyContent = EmptyContentHelper.InvalidSearch;
                    break;
                }
                LoadMoreItems();
                EmptyContent = _searchItems.Count == 0 ? EmptyContentHelper.NoMatchesFound : null;
                break;
            case OneNoteExplorerFilters.TitleSearchFilterId:
                _searchItems.Clear();
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    EmptyContent = EmptyContentHelper.EmptySearch;
                    break;
                }
                LoadMoreItems();
                EmptyContent = _searchItems.Count == 0 ? EmptyContentHelper.NoMatchesFound : null;
                break;
            case OneNoteExplorerFilters.DefaultFilterId:
            default:
                _searchItems.Clear();
                _searchItems.Add(new OpenOrCreateItemListItem(_item));
                var results = _item.Children.Select(i => new OneNoteItemListItem(i, false));
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    _searchItems.AddRange(results);
                    break;
                }
                _searchItems.AddRange(ListHelpers.FilterList(results, SearchText).Cast<ListItem>());
                break;
        }
        RaiseItemsChanged();
    }

    private IEnumerable<ListItem> SearchAction(string search, int skip, int take)
    {
        return (Filters?.CurrentFilterId) switch
        {
            OneNoteExplorerFilters.ScopeSearchFilterId => OneNoteHelper.FindPages(search, _item).Skip(skip).Take(take).Select(x => new OneNoteItemListItem(x, true, true)),
            OneNoteExplorerFilters.TitleSearchFilterId => ListHelpers.FilterList(_item.Descendants(), search, ScoreFunction).Select(x => new OneNoteItemListItem(x, true, true)),
            _ => []
        };
    }

    private void LoadMoreItems()
    {
        IsLoading = true;
        var results = SearchAction(SearchText, _searchItems.Count, _resultsPerLoad);
        var preCount = _searchItems.Count;
        _searchItems.AddRange(results);
        var postCount = _searchItems.Count;
        HasMoreItems = (postCount - preCount) == _resultsPerLoad;
        IsLoading = false;
    }
    public override void LoadMore()
    {
        LoadMoreItems();
        RaiseItemsChanged(_searchItems.Count);
    }

    private static int ScoreFunction(string search, IOneNoteItem item) => StringMatcher.FuzzySearch(search, item.Name).Score;
    private partial class OneNoteExplorerFilters : Filters
    {
        public const string DefaultFilterId = "default";
        public const string ScopeSearchFilterId = "scope";
        public const string TitleSearchFilterId = "title";
        public override IFilterItem[] GetFilters() =>
        [
            new Filter { Id = DefaultFilterId, Name = Resources.DefaultFilter},// Viewing direct children
            new Filter { Id = ScopeSearchFilterId, Name = Resources.Pages, Icon = Icons.Page },
            new Filter { Id = TitleSearchFilterId, Name = Resources.Titles, Icon = Icons.Title },
        ];
    }
}
