using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.ListItems;

internal sealed partial class OpenOneNoteListItem : ListItem
{
    public OpenOneNoteListItem(IExternalItemsChanged listPage, Root root)
    {
        Title = Resources.OpenOneNote;
        Subtitle = Resources.OrCreateNotebook;
        Icon = Icons.OneNote;
        Command = new OpenOneNoteCommand();
        MoreCommands = [new CreateItemFormPage.Notebook(root, listPage).ToContextItem()];
    }
}