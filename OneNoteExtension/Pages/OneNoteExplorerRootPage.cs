using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerRootPage : ListPage
{
    public OneNoteExplorerRootPage()
    {
        Title = Resources.OneNoteExplorer;
        Icon = Icons.OneNoteExplorer;
        Name = Title;
    }

    public override IListItem[] GetItems()
    {
        IsLoading = true;
        var notebooks = OneNoteHelper.GetFullHierarchy().Notebooks.Select(n => new OneNoteItemListItem(n, false)).ToArray();
        IsLoading = false;
        return notebooks;
    }
}
