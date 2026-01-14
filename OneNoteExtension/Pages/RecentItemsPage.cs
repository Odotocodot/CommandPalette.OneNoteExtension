using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
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
    }

    public override IListItem[] GetItems() => OneNote.GetFullHierarchy().Notebooks
                                                     .GetAllPages()
                                                     .OrderByDescending(p => p.LastModified)
                                                     .Take(20)
                                                     .Select(p => new SearchPageListItem(p, Icons.RecentPage))
                                                     .ToArray();
}
