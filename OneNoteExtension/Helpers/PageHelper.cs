using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.ListItems;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Page = Microsoft.CommandPalette.Extensions.Toolkit.Page;

namespace OneNoteExtension.Helpers;

internal static class PageHelper
{
    public static CommandContextItem ToContextItem(this ICommand command, IContextItem[]? moreCommands = null)
    {
        if(command is Page page)
        {
            page.Name = page.Title;
        }
        return new CommandContextItem(command) { MoreCommands = moreCommands ?? [] };
    }

    public static CommandContextItem[] ToContextItems(this ICommand[] commands, IContextItem[]? moreCommands = null)
    {
        return Array.ConvertAll(commands, c => c.ToContextItem(moreCommands));
    }

    public static IEnumerable<OneNoteItemListItem> AsListItems(this IEnumerable<IOneNoteItem> source, bool addSubtitle, bool commandIsOpen, IconInfo? icon = null)
    {
        return source.Where(item => SettingsManager.Instance.ShowRecycleBinEntries || !item.IsInRecycleBin())
                     .Select(item => new OneNoteItemListItem(item, addSubtitle, commandIsOpen, icon));
    }

    public static IEnumerable<IOneNoteItem> FilterItems(this IEnumerable<IOneNoteItem> source, string search)
    {
        return ListHelpers.FilterList(source, search, ScoreFunction);
        static int ScoreFunction(string search, IOneNoteItem item) => StringMatcher.FuzzySearch(search, item.Name).Score;
    }

    public static ICommand[] GetMoreCommands(IOneNoteItem item, IExternalItemsChanged? listPage, bool includeDefault = false)
    {
        var moreCommands = new List<ICommand>();
        if (includeDefault)
        {
            moreCommands.Add(new OpenInOneNoteCommand(item, false));
        }
        moreCommands.Add(new OpenInOneNoteCommand(item, true));
        if (listPage != null)
        {
            switch (item)
            {
                case INotebookOrSectionGroup notebookOrSectionGroup:
                    moreCommands.Add(new CreateItemFormPage.SectionGroup(notebookOrSectionGroup, listPage));
                    moreCommands.Add(new CreateItemFormPage.Section(notebookOrSectionGroup, listPage));
                    break;
                case Section section:
                    moreCommands.Add(new CreateItemFormPage.Page(section, listPage));
                    break;
            }
        }
        moreCommands.Add(new CopyLinkToClipboardCommand(item));

        return moreCommands.ToArray();
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