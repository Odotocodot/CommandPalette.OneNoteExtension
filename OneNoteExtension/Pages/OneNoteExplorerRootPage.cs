using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerRootPage : DynamicListPage, IExternalItemsChanged
{
    public OneNoteExplorerRootPage()
    {
        Name = Title = Resources.OneNoteExplorer;
        Icon = Icons.OneNoteExplorer;
    }

    public override IListItem[] GetItems()
    {
        IsLoading = true;

        var root = OneNoteHelper.GetFullHierarchy();
        var notebooks = root.Notebooks;
        ListItem[] results = string.IsNullOrWhiteSpace(SearchText)
            ? [new OpenOneNoteListItem(this, root), .. notebooks.AsListItems(false, false)]
            : [.. notebooks.FilterItems(SearchText).AsListItems(false, false)];

        if (results.Length == 0)
        {
            EmptyContent = EmptyContentHelper.GetNotMatchesFoundWithCommands(this, root);
        }
        IsLoading = false;
        return results;
    }

    public void RaiseItemsChangedExternal() => RaiseItemsChanged();

    public override void UpdateSearchText(string oldSearch, string newSearch) => RaiseItemsChanged();
}
