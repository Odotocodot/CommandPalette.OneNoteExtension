using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.ListItems;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
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

    public static IEnumerable<OneNoteItemListItem> AsListItems(this IEnumerable<IOneNoteItem> source, bool addSubtitle, bool commandIsOpen, IconInfo? icon = null)
    {
        return source.Select(item => new OneNoteItemListItem(item, addSubtitle, commandIsOpen, icon));
    }

    public static IEnumerable<IOneNoteItem> FilterItems(this IEnumerable<IOneNoteItem> source, string search)
    {
        return ListHelpers.FilterList(source, search, ScoreFunction);
        static int ScoreFunction(string search, IOneNoteItem item) => StringMatcher.FuzzySearch(search, item.Name).Score;
    }

    public static List<IContextItem> GetMoreCommands(IExternalItemsChanged listPage, IOneNoteItem item, bool includeDefault = false)
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
                moreCommands.Add(new CreateItemFormPage.SectionGroup(notebookOrSectionGroup, listPage).ToContextItem());
                moreCommands.Add(new CreateItemFormPage.Section(notebookOrSectionGroup, listPage).ToContextItem());
                break;
            case Section section:
                moreCommands.Add(new CreateItemFormPage.Page(section, listPage).ToContextItem());
                break;
        }

        return moreCommands;
    }

    public static string GetSubtitle(IOneNoteItem item, bool includeSelf)
    {
        const string separator = " > ";
        var path = item.GetRelativePath(false, separator);
        return includeSelf
            ? path
            : path[..^(item.Name.Length + separator.Length)];
    }
}