using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class RecentItemsPage : ListPage
{
    public RecentItemsPage()
    {
        Icon = Icons.RecentPage;
        Name = Title = Resources.ViewRecentOneNotePages;
        EmptyContent = EmptyContentHelper.NoMatchesFound;
    }

    public override IListItem[] GetItems() => OneNoteHelper.GetFullHierarchy().Notebooks
                                                           .GetAllPages()
                                                           .OrderByDescending(p => p.LastModified)
                                                           .Take(20)
                                                           .Select(p => new OneNoteItemListItem(p, Icons.RecentPage, true))
                                                           .ToArray();
}
