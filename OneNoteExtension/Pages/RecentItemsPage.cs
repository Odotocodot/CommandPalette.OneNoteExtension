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
        Title = Resources.ViewRecentOneNotePages;
        Name = Resources.Open;
        EmptyContent = PageHelper.EmptyContents.NoMatchesFound;
    }

    public override IListItem[] GetItems() => OneNote.GetFullHierarchy().Notebooks
                                                     .GetAllPages()
                                                     .OrderByDescending(p => p.LastModified)
                                                     .Take(20)
                                                     .Select(p => new OneNoteItemListItem(p, Icons.RecentPage, true))
                                                     .ToArray();
}
