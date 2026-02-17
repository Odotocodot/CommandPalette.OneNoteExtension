using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Pages;
namespace OneNoteExtension.Helpers;

//For top level commands, the Name should be equal to the Title property
internal static class TopLevelCommands
{
    public static Page[] Commands =>
    [
        new DefaultSearchPage(),
        new RecentItemsPage(),
        new OneNoteExplorerRootPage(),
        new CreateItemFormPage.QuickNote(),
    ];
}
