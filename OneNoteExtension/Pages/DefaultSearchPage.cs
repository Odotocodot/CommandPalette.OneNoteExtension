using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;
using System.Collections.Generic;
using System.Linq;

namespace OneNoteExtension.Pages;

internal sealed partial class DefaultSearchPage : DynamicListPageExt
{
    private readonly int _resultsPerLoad = 25;
    private readonly List<ListItem> _searchItems = [];

    public DefaultSearchPage()
    {
        Name = Title = Resources.SearchOneNotePages;
        Icon = Icons.Search;
        EmptyContent = EmptyContentHelper.EmptySearch;
        HasMoreItems = true;
        PageLoaded += OneNoteHelper.InitComObject;
        PageUnloaded += _searchItems.Clear;
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        Search();
        RaiseItemsChanged();
    }

    private void Search()
    {
        _searchItems.Clear();
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            EmptyContent = EmptyContentHelper.EmptySearch;
            return;
        }

        if (!char.IsLetterOrDigit(SearchText[0]))
        {
            EmptyContent = EmptyContentHelper.InvalidSearch;
            return;
        }

        LoadMoreItems();

        EmptyContent = _searchItems.Count == 0 ? EmptyContentHelper.NoMatchesFound : null;
    }

    private static IEnumerable<ListItem> SearchAction(string search, int skip, int take)
    {
        return OneNoteHelper.FindPages(search).Skip(skip).Take(take).Select(x => new OneNoteItemListItem(x, true, true));
    }

    public override IListItem[] GetItems() => [.. _searchItems];
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
}
