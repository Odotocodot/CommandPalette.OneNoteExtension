// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Properties;
namespace OneNoteExtension.ListItems;

internal partial class SearchPageListItem : ListItem
{
    private readonly LinqToOneNote.Page page;

    public SearchPageListItem(LinqToOneNote.Page page)
    {
        Task.Run(GetSubTitle);
        this.page = page;
        var tags = new List<Tag>();
        if (page.IsUnread)
        {
            tags.Add(new Tag(Resources.Unread));
        }
        Title = page.Name;
        Icon = Icons.Page;
        Command = new OpenInOneNoteCommand(page);
        Tags = [.. tags];
        Details = new Details
        {
            Title = page.Name,
            Metadata = //Use Markdown instead?
            [
                new DetailsElement
                {
                    Key = Resources.Created,
                    Data = new DetailsTags { Tags = [new Tag(page.Created.ToString(CultureInfo.CurrentCulture))] }
                },
                new DetailsElement
                {
                    Key = Resources.LastModified,
                    Data = new DetailsTags { Tags = [new Tag(page.LastModified.ToString(CultureInfo.CurrentCulture))] }
                },
                new DetailsElement
                {
                    Key = Resources.Commands,
                    Data = new DetailsCommands { Commands = OpenInOneNoteCommand.GetAll(page) }
                }
            ]
        };
    }

    private void GetSubTitle()
    {
        const string separator = " > ";
        var path = page.GetRelativePath(false, separator);
        var offset = page.Name.Length + separator.Length;
        Subtitle = path[..^offset];
    }
}
