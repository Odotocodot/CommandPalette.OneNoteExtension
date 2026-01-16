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
        Icon = Icons.OneNote
        //MoreCommands =
        //[
        //    new CommandContextItem(new NotebookExplorerPage()) { Title = "Goto Notebook Explorer" }
        //]
    };

    public static CommandItem NoChildren { get; } = new()
    {
        Title = Resources.OneNoteItemNoChildren,
        Icon = Icons.OneNote,
    };
}