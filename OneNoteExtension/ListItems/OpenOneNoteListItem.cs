using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Properties;

namespace OneNoteExtension.ListItems;

internal partial class OpenOneNoteListItem : ListItem
{
    public OpenOneNoteListItem(Root root)
    {
        Title = Resources.OpenOneNote;
        Subtitle = Resources.OrCreateNotebook;
        Icon = Icons.OneNote;
        Command = new OpenOneNoteCommand();
        MoreCommands = [new CreateItemFormPage.Notebook(root).ToContextItem()];
    }
}