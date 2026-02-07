using System.Collections.Generic;
using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class RecentItemsPage : LoadMorePage
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

    protected override IEnumerable<ListItem> GetItemsAction(string search)
    {
        return OneNoteHelper.GetFullHierarchy().Notebooks
                            .GetAllPages()
                            .OrderByDescending(p => p.LastModified)
                            .AsListItems(true, true, Icons.RecentPage);
    }
}
