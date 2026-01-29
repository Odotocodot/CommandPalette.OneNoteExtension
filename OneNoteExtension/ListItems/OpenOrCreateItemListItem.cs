using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using OneNoteExtension.Properties;

namespace OneNoteExtension.ListItems;

internal partial class OpenOrCreateItemListItem : OneNoteItemListItem
{
    private static readonly CompositeFormat openXInOneNote = CompositeFormat.Parse(Resources.OpenXInOneNote);
    public OpenOrCreateItemListItem(IOneNoteItem item) : base(item, false, true)
    {
        Title = string.Format(CultureInfo.CurrentCulture, openXInOneNote, item.Name);
        Subtitle = Resources.OpenOrCreateItemListItemSubtitle;
        List<IContextItem> moreCommands = [new CommandContextItem(new OpenInOneNoteCommand(item, true))];
        switch (item)
        {
            case INotebookOrSectionGroup notebookOrSectionGroup:
                moreCommands.Add(new CreateItemFormPage.SectionGroup(notebookOrSectionGroup).ToContextItem());
                moreCommands.Add(new CreateItemFormPage.Section(notebookOrSectionGroup).ToContextItem());
                break;
            case Section section:
                moreCommands.Add(new CreateItemFormPage.Page(section).ToContextItem());
                break;
        }
        MoreCommands = moreCommands.ToArray();
    }
}