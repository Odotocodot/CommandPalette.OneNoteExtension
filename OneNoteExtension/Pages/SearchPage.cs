using Microsoft.CommandPalette.Extensions;
using OneNoteExtension.Helpers;

namespace OneNoteExtension.Pages;

internal abstract partial class SearchPage : LoadMorePage, IDynamicListPage
{
    public override string SearchText
    {
        get => base.SearchText;
        set
        {
            SetSearchNoUpdate(value);
            UpdateSearchText(base.SearchText, value);
        }
    }

    protected void OnSearchChanged(string search, bool invalidCharCheck)
    {
        _searchItems.Clear();
        if (string.IsNullOrWhiteSpace(search))
        {
            EmptyContent = EmptyContentHelper.EmptySearch;
            return;
        }

        if (invalidCharCheck && !char.IsLetterOrDigit(search[0]))
        {
            EmptyContent = EmptyContentHelper.InvalidSearch;
            return;
        }

        GetMoreItems(search);

        EmptyContent = _searchItems.Count == 0 ? EmptyContentHelper.NoMatchesFound : null;
    }

    public override IListItem[] GetItems() => [.. _searchItems];

    public abstract void UpdateSearchText(string oldSearch, string newSearch);
}
