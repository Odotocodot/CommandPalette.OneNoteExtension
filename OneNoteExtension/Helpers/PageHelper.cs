using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Helpers;

internal static class PageHelper
{
    public static CommandItem ToCommandItem(this Page page, string subtitle = "") => new(page)
    {
        Title = page.Title,
        Subtitle = subtitle
    };

    public static class EmptyContents
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
}
