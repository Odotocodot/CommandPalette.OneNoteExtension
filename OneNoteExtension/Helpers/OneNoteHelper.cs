using LinqToOneNote;

namespace OneNoteExtension.Helpers;

internal static class OneNoteHelper
{
    public static void OpenInOneNote(string itemId, bool newWindow)
    {
        OneNote.Open(itemId, newWindow);
        NativeMethods.BringProcessToFront("onenote");
    }
}
