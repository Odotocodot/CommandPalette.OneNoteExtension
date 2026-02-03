using System.Collections.Generic;
using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class RecentItemsPage : ListPageExt
{
    private const int _resultsPerLoad = 25;
    private readonly List<ListItem> _searchItems = [];

    public RecentItemsPage()
    {
        Icon = Icons.RecentPage;
        Name = Title = Resources.ViewRecentOneNotePages;
        EmptyContent = EmptyContentHelper.NoMatchesFound;
        HasMoreItems = true;
        PageLoaded += () =>
        {
            _searchItems.Clear();
            RaiseItemsChanged();
        };
    }

    public override IListItem[] GetItems()
    {
        if (_searchItems.Count == 0)
        {
            LoadMore();
        }
        return [.. _searchItems];
    }

    public override void LoadMore()
    {
        var results = OneNoteHelper.GetFullHierarchy().Notebooks
                                   .GetAllPages()
                                   .OrderByDescending(p => p.LastModified)
                                   .Skip(_searchItems.Count)
                                   .Take(_resultsPerLoad)
                                   .Select(p => new OneNoteItemListItem(p, Icons.RecentPage, true));
        var preCount = _searchItems.Count;
        _searchItems.AddRange(results);
        var postCount = _searchItems.Count;
        HasMoreItems = (postCount - preCount) == _resultsPerLoad;
        RaiseItemsChanged(_searchItems.Count);
    }
}
