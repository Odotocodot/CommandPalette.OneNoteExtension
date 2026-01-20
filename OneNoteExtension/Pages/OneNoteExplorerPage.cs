using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerPage : SearchPage
{
    private readonly IOneNoteItem _item;

    public OneNoteExplorerPage(IOneNoteItem item)
    {
        _item = item;
        if(item is not Section)
        {
            var filters = new OneNoteExplorerFilters();
            filters.PropChanged += (_, _) => RaiseItemsChanged();
            Filters = filters;
        }
        var relativePath = OneNoteItemHelper.GetSubtitle(item, true);
        Title = item is Notebook notebook
            ? $"{Resources.OneNoteExplorer} | {notebook.DisplayName}"
            : $"{Resources.OneNoteExplorer} | {relativePath}";
        Icon = Icons.GetIcon(item);
        Name = Resources.Enter;
        EmptyContent = EmptyContentHelper.NoChildren;
    }

    public override IListItem[] GetItems()
    {
        switch (Filters?.CurrentFilterId)
        {
            case OneNoteExplorerFilters.ScopeSearchFilterId:
                return Search(search => OneNoteHelper.FindPages(search, _item), true, true);
            case OneNoteExplorerFilters.TitleSearchFilterId:
                return Search(search => ListHelpers.FilterList(_item.Descendants(), search, (search, child) => StringMatcher.FuzzySearch(search, child.Name).Score), false, true, true);
            default:
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    var items = _item.Children.Select(i => new OneNoteItemListItem(i, false)).ToArray();
                    if (items.Length == 0)
                    {
                        EmptyContent = EmptyContentHelper.NoChildren;
                    }
                    return items;
                }
                return Search(search => ListHelpers.FilterList(_item.Children, search, (search, child) => StringMatcher.FuzzySearch(search, child.Name).Score), false, false);
        }
    }

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
