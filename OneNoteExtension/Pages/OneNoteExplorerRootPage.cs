using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
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
        var _root = OneNote.GetFullHierarchy(); //TODO: update when changed 
        IsLoading = false;
        return _root.Notebooks.Select(n => new OneNoteItemListItem(n, false)).ToArray();
    }
}
