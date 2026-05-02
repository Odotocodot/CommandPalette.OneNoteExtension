using System.Collections.Generic;
using System.Linq;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Pages.Core;

/// <summary>
/// A <see cref="Page"/> to make pagination <i>easier</i> to implement. Works, but could probably use a refactor.
/// </summary>
internal abstract partial class LoadMorePage : ListPageExt
{
    protected readonly List<ListItem> _searchItems = [];
    protected virtual int ResultsPerLoad { get; set; } = 25;

    /// <summary>
    /// Called when needing more results i.e. in <see cref="LoadMore"/>. Deals with pagination
    /// </summary>
    /// <param name="search"></param>
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

    /// <summary>
    /// The action that the results to be paginated in <see cref="GetMoreItems(string)"/>
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    protected abstract IEnumerable<ListItem> GetItemsAction(string search);

    public override void LoadMore()
    {
        GetMoreItems(SearchText);
        RaiseItemsChanged(_searchItems.Count);
    }
}

