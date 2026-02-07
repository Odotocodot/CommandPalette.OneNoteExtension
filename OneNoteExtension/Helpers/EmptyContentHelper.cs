using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Pages;
using OneNoteExtension.Properties;
using System;

namespace OneNoteExtension.Helpers;

internal static class EmptyContentHelper
{
    public static CommandItem NoMatchesFound { get; } = new()
    {
        Title = Resources.NoMatchesFound,
        Icon = Icons.OneNote
    };

    public static CommandItem GetNotMatchesFoundWithCommands(IOneNoteItem item) => new()
    {
        Title = NoMatchesFound.Title,
        Icon = NoMatchesFound.Icon,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands = [.. PageHelper.GetMoreCommands(item, true)]
    };

    public static CommandItem GetNotMatchesFoundWithCommands(Root root) => new()
    {
        Title = NoMatchesFound.Title,
        Icon = NoMatchesFound.Icon,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands =
        [
            new OpenOneNoteCommand().ToContextItem(),
            new CreateItemFormPage.Notebook(root).ToContextItem()
        ]
    };

    public static CommandItem EmptySearch { get; } = new()
    {
        Title = Resources.SearchPageDefaultEmptyContent,
        Icon = Icons.OneNote,
        Subtitle = Resources.SeeMoreCommands,
        MoreCommands =
        [
            new OpenOneNoteCommand().ToContextItem(),
            TopLevelCommands.RecentPages.ToContextItem(),
            TopLevelCommands.QuickNote.ToContextItem(),
            TopLevelCommands.OneNoteExplorer.ToContextItem(),
        ]
    };

    [Obsolete("Remove not used any more")]
    public static CommandItem NoChildren { get; } = new()
    {
        Title = Resources.OneNoteItemNoChildren,
        Icon = Icons.OneNote,
    };
    public static CommandItem InvalidSearch { get; } = new()
    {
        Title = Resources.InvalidSearch,
        Icon = Icons.Invalid
    };
}