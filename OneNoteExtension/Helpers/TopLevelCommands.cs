using OneNoteExtension.Pages;
namespace OneNoteExtension.Helpers;

//For top level commands, the Name should be equal to the Title property
internal static class TopLevelCommands
{
    public static DefaultSearchPage DefaultSearch => new();
    public static RecentItemsPage RecentPages => new();
    public static OneNoteExplorerRootPage OneNoteExplorer => new();
    public static CreateItemFormPage.QuickNote QuickNote => new();
}
