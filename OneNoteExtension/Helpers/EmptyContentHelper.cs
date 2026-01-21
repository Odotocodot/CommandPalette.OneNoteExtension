using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Helpers;

internal static class EmptyContentHelper
{
    public static CommandItem NoMatchesFound { get; } = new()
    {
        Title = Resources.NoMatchesFound,
        Icon = Icons.OneNote
    };

    public static CommandItem EmptySearch { get; } = new()
    {
        Title = Resources.SearchPageDefaultEmptyContent,
        Icon = Icons.OneNote,
        MoreCommands =
        [
            ToContextItem(TopLevelCommandsHelper.RecentPages),
            ToContextItem(TopLevelCommandsHelper.QuickNote),
            ToContextItem(TopLevelCommandsHelper.OneNoteExplorer),
        ]

    };
    private static CommandContextItem ToContextItem(Page page) => new(page) { Title = page.Title };

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