// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.ListItems;
using OneNoteExtension.Properties;
namespace OneNoteExtension.Pages;

internal sealed partial class DefaultSearchPage : DynamicListPage
{
    private IEnumerable<ListItem> _items = [];

    public DefaultSearchPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = Resources.SearchOneNotePages;
        Name = Resources.Open;
        Icon = Icons.Search;
        EmptyContent = DefaultEmptyContent();
    }
    private static CommandItem DefaultEmptyContent() => new()
    {
        Title = Resources.SearchPageDefaultEmptyContent,
        Icon = Icons.OneNote
        //MoreCommands =
        //[
        //    new CommandContextItem(new NotebookExplorerPage()) { Title = "Goto Notebook Explorer" }
        //]
    };

    public override IListItem[] GetItems() => [.. _items];

    private (IEnumerable<ListItem> items, ICommandItem? emptyContent) Search(string search) //Can be moved to another class
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return ([], DefaultEmptyContent());
        }

        if (!char.IsLetterOrDigit(search[0]))
        {
            return ([], new CommandItem
            {
                Title = Resources.InvalidSearch,
                Icon = Icons.Invalid
            });
        }
        IsLoading = true;
        var results = OneNote.FindPages(search).Select(x => new SearchPageListItem(x));
        IsLoading = false;
        if (results.Any())
        {
            return (results, null);
        }

        return ([], new CommandItem
        {
            Title = Resources.NoPagesFound,
            Icon = Icons.Search
        });
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        var (items, emptyContent) = Search(newSearch);
        EmptyContent = emptyContent;
        _items = items;
        RaiseItemsChanged();
    }
}
