using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal sealed partial class DefaultSearchPage : SearchPage
{
    public DefaultSearchPage()
    {
        Name = Title = Resources.SearchOneNotePages;
        Icon = Icons.SearchPages;
        EmptyContent = EmptyContentHelper.EmptySearch;
        HasMoreItems = true;
        PageLoaded += OneNoteHelper.InitComObject;
        PageUnloaded += _searchItems.Clear;
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        OnSearchChanged(newSearch, true);
        RaiseItemsChanged();
    }

    protected override IEnumerable<ListItem> GetItemsAction(string search) => OneNoteHelper.FindPages(search).AsListItems(true, true);
}
