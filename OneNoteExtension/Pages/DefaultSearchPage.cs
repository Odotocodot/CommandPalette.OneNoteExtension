using Microsoft.CommandPalette.Extensions;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal sealed partial class DefaultSearchPage : SearchPage
{
    public DefaultSearchPage()
    {
        Name = Title = Resources.SearchOneNotePages;
        Icon = Icons.Search;
        EmptyContent = EmptyContentHelper.EmptySearch;
    }

    public override IListItem[] GetItems() => Search(OneNoteHelper.FindPages, true, true);
}
