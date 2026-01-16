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
}
