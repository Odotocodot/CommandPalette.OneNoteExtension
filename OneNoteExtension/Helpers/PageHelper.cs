using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Helpers;

public static class PageHelper
{
    public static CommandContextItem ToContextItem(this Page page, string subtitle = "")
    {
        page.Name = page.Title;
        return new CommandContextItem(page) { Subtitle = subtitle };
    }
}