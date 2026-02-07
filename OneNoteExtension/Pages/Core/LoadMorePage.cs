using System.Collections.Generic;
using System.Linq;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Pages.Core;

internal abstract partial class LoadMorePage : ListPageExt
{
    protected readonly List<ListItem> _searchItems = [];
    protected virtual int ResultsPerLoad { get; set; } = 25;

    protected void GetMoreItems(string search)
    {
        IsLoading = true;
        var results = GetItemsAction(search).Skip(_searchItems.Count).Take(ResultsPerLoad);
        var preCount = _searchItems.Count;
        _searchItems.AddRange(results);
        var postCount = _searchItems.Count;
        HasMoreItems = (postCount - preCount) == ResultsPerLoad;
        IsLoading = false;
    }

    protected abstract IEnumerable<ListItem> GetItemsAction(string search);

    public override void LoadMore()
    {
        GetMoreItems(SearchText);
        RaiseItemsChanged(_searchItems.Count);
    }
}

