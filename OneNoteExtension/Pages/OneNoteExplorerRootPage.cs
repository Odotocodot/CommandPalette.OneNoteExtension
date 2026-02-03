using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal partial class OneNoteExplorerRootPage : DynamicListPageExt
{
    public OneNoteExplorerRootPage()
    {
        Name = Title = Resources.OneNoteExplorer;
        Icon = Icons.OneNoteExplorer;
        PageLoaded += () => RaiseItemsChanged();
    }

    public override IListItem[] GetItems()
    {
        IsLoading = true;

        var root = OneNoteHelper.GetFullHierarchy();
        var notebooks = root.Notebooks.Select(n => new OneNoteItemListItem(n, false));

        IsLoading = false;

        return string.IsNullOrWhiteSpace(SearchText)
            ? [new OpenOneNoteListItem(root), .. notebooks]
            : [.. ListHelpers.FilterList(notebooks, SearchText, ListHelpers.ScoreListItem)];
    }

    public override void UpdateSearchText(string oldSearch, string newSearch) => RaiseItemsChanged();
}
