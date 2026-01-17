using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.ListItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneNoteExtension.Pages;

internal abstract partial class SearchPage : DynamicListPage
{
    protected ListItem[] Search(Func<string, IEnumerable<IOneNoteItem>> searchAction, bool invalidCharCheck, bool addSubtitle, bool commandIsOpen = false)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            EmptyContent = EmptyContentHelper.EmptySearch;
            return [];
        }

        if (invalidCharCheck && !char.IsLetterOrDigit(SearchText[0]))
        {
            EmptyContent = EmptyContentHelper.InvalidSearch;
            return [];
        }
        IsLoading = true;
        var items = searchAction(SearchText).Select(x => new OneNoteItemListItem(x, addSubtitle, commandIsOpen)).ToArray();
        IsLoading = false;

        if (items.Length == 0)
        {
            EmptyContent = EmptyContentHelper.NoMatchesFound;
            return [];
        }

        EmptyContent = null;
        return items;
    }

    public override void UpdateSearchText(string oldSearch, string newSearch) => RaiseItemsChanged();
}
