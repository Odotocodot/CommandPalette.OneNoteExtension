using System;
using System.Collections.Generic;
using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class RecentItemsPage : SearchPage
{
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
    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        _searchItems.Clear();
        GetMoreItems(newSearch);
        RaiseItemsChanged();
    }

    protected override IEnumerable<ListItem> GetItemsAction(string search)
    {
        var pages = OneNoteHelper.GetFullHierarchy().Notebooks.GetAllPages(); //Maybe cache for performance?
        var results = string.IsNullOrWhiteSpace(search)
            ? pages 
            : pages.Where(pg => FuzzyStringMatcher.ScoreFuzzy(search, pg.Name) > 0);
        return results.OrderByDescending(static pg => pg.LastModified).ToListItems(true, true, true, Icons.RecentPage);
    }
}
