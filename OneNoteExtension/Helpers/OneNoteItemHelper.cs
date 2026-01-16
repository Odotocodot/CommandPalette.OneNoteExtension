using LinqToOneNote;

namespace OneNoteExtension.Helpers;

internal static class OneNoteItemHelper
{
    public static string GetSubtitle(IOneNoteItem item, bool includeSelf)
    {
        const string separator = " > ";
        var path = item.GetRelativePath(false, separator);
        return includeSelf
            ? path
            : path[..^(item.Name.Length + separator.Length)];
    }
}
