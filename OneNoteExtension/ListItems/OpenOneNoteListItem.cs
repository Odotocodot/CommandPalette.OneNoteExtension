using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Properties;
using System;
using System.Linq;

namespace OneNoteExtension.ListItems;

internal partial class OpenOneNoteListItem : ListItem
{
    public OpenOneNoteListItem(Root root)
    {
        Title = Resources.OpenOneNote;
        Subtitle = Resources.OpenOrCreateItemListItemSubtitle;
        Icon = Icons.OneNote;
        MoreCommands = [new CreateItemFormPage.Notebook(root).ToContextItem()];
        Command = new AnonymousCommand(() =>
        {
            try
            {
                ShellHelpers.OpenInShell("C:\\Program Files\\Microsoft Office\\Root\\Office16\\ONENOTE.EXE", runWithHiddenWindow: true);
            }
            catch (Exception)
            {
                var mostRecentPage = OneNoteHelper.GetFullHierarchy().Notebooks
                                                  .GetAllPages()
                                                  .Where(i => !i.IsInRecycleBin)
                                                  .OrderByDescending(pg => pg.LastModified)
                                                  .First();

                OneNoteHelper.OpenInOneNote(mostRecentPage.Id, false);
            }
        })
        {
            Icon = Icons.OneNote,
            Name = Resources.Open
        };
    }
}