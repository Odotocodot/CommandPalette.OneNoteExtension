using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.ListItems;
using OneNoteExtension.Pages;
using System.Collections.Generic;
using System.Linq;
using Page = Microsoft.CommandPalette.Extensions.Toolkit.Page;

namespace OneNoteExtension.Helpers;

internal static class PageHelper
{
    public static CommandContextItem ToContextItem(this Page page, string subtitle = "")
    {
        page.Name = page.Title;
        return new CommandContextItem(page) { Subtitle = subtitle };
    }

    public static CommandContextItem ToContextItem(this Command command, string subtitle = "")
    {
        return new CommandContextItem(command) { Subtitle = subtitle };
    }
    public static List<IContextItem> GetMoreCommands(IOneNoteItem item, bool includeDefault = false)
    {
        var moreCommands = new List<IContextItem>();
        if (includeDefault)
        {
            moreCommands.Add(new OpenInOneNoteCommand(item, false).ToContextItem());
        }
        moreCommands.Add(new OpenInOneNoteCommand(item, true).ToContextItem());
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

        return moreCommands;
    }
}