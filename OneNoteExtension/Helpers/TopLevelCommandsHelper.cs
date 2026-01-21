using OneNoteExtension.Pages;
namespace OneNoteExtension.Helpers;

internal static class TopLevelCommandsHelper
{
    public static DefaultSearchPage DefaultSearch => new();
    public static RecentItemsPage RecentPages => new();
    public static OneNoteExplorerRootPage OneNoteExplorer => new();
    public static QuickNoteFormPage QuickNote => new();
}
