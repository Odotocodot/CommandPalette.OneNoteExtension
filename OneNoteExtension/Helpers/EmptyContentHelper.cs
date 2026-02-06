using Microsoft.CommandPalette.Extensions.Toolkit;
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

    public static CommandItem NotMatchesFoundWithCommands { get; } = new()
    {
        Title = NoMatchesFound.Title,
        Icon = NoMatchesFound.Icon,
        //MoreCommands = [] //TODO:
    };

    public static CommandItem EmptySearch { get; } = new()
    {
        Title = Resources.SearchPageDefaultEmptyContent,
        Icon = Icons.OneNote,
        MoreCommands =
        [
            TopLevelCommands.RecentPages.ToContextItem(),
            TopLevelCommands.QuickNote.ToContextItem(),
            TopLevelCommands.OneNoteExplorer.ToContextItem(),
        ]

    };

    [Obsolete("Remove Not Needed any more")]
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