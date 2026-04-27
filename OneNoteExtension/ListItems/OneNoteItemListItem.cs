using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
using Resources = OneNoteExtension.Properties.Resources;

namespace OneNoteExtension.ListItems;

internal partial class OneNoteItemListItem : ListItem
{
    private static readonly CompositeFormat openXInOneNote = CompositeFormat.Parse(Resources.OpenXInOneNote);
    public OneNoteItemListItem(IExternalItemsChanged listPage, IOneNoteItem item) : this(item, false, true, false)
    {
        Title = string.Format(CultureInfo.CurrentCulture, openXInOneNote, item.Name);
        Subtitle = Resources.SeeMoreCommands;
        MoreCommands = PageHelper.GetMoreCommands(item, listPage).ToContextItems();
    }

    public OneNoteItemListItem(IOneNoteItem item, bool addSubtitle, bool commandIsOpenInOneNote, bool addLastModifiedTag, IconInfo? icon = null)
    {
        //Tags
        var tags = new List<Tag>(4);
        if (item.IsUnread)
        {
            tags.Add(new Tag { Icon = Icons.UnreadChanges, ToolTip = Resources.UnreadToolTip });
        }
        if (item.IsInRecycleBin())
        {
            tags.Add(new Tag { Text = "♻", ToolTip = Resources.RecycleBinToolTip });
        }
        var section = item as LinqToOneNote.Section;
        if (section?.Encrypted == true)
        {
            tags.Add(section.Locked
                ? new Tag { Icon = Icons.Locked, ToolTip = Resources.LockedToolTip }
                : new Tag { Icon = Icons.Unlocked, ToolTip = Resources.UnlockedToolTip }
            );
        }
        if (addLastModifiedTag)
        {
            tags.Add(new Tag(item.LastModified.Humanize(culture: CultureInfo.CurrentCulture)) { ToolTip = Resources.LastModified });
        }

        //Subtitle
        if (addSubtitle)
        {
            Task.Run(() => Subtitle = PageHelper.GetSubtitle(item, false));
        }

        //Command 
        var page = item as LinqToOneNote.Page;
        Command command;
        if (commandIsOpenInOneNote || page != null || section is { Encrypted: true, Locked: true })
        {
            command = new OpenInOneNoteCommand(item);
        }
        else //In the onenote explorer but not searching
        {
            command = new OneNoteExplorerPage(item);
        }

        //Details Metadata
        var metadata = new List<DetailsElement>(6);
        if (page != null)
        {
            metadata.Add(new DetailsElement
            {
                Key = Resources.Created,
                Data = new DetailsTags { Tags = [new Tag(page.Created.ToString(CultureInfo.CurrentCulture))] }
            });
        }

        metadata.Add(new DetailsElement
        {
            Key = Resources.LastModified,
            Data = new DetailsTags { Tags = [new Tag(item.LastModified.ToString(CultureInfo.CurrentCulture))] }
        });

        if (page == null)
        {
            var childrenTags = new List<Tag>(3);
            if (item is INotebookOrSectionGroup notebookOrSectionGroup)
            {
                childrenTags.Add(new Tag(Resources.Sections.ToLower(CultureInfo.CurrentCulture).ToQuantity(notebookOrSectionGroup.Sections.Count, null, CultureInfo.CurrentCulture)));
                childrenTags.Add(new Tag(Resources.SectionGroups.ToLower(CultureInfo.CurrentCulture).ToQuantity(notebookOrSectionGroup.SectionGroups.Count, null, CultureInfo.CurrentCulture)));
                // Could add tag for total pages
            }
            if (section != null)
            {
                childrenTags.Add(new Tag(Resources.Pages.ToLower(CultureInfo.CurrentCulture).ToQuantity(section.Pages.Count, null, CultureInfo.CurrentCulture)));
            }
            metadata.Add(new DetailsElement
            {
                Key = Resources.Children,
                Data = new DetailsTags { Tags = childrenTags.ToArray() }
            });
        }

        if (item is IHasPath hasPath)
        {
            metadata.Add(new DetailsElement
            {
                Key = Resources.Path,
                Data = new DetailsLink(hasPath.Path)
            });
        }

        if (tags.Count > 0)
        {
            metadata.Add(new DetailsElement
            {
                Key = Resources.Tags,
                Data = new DetailsTags { Tags = tags.ToArray() }
            });
        }

        metadata.Add(new DetailsElement
        {
            Key = Resources.Commands,
            Data = new DetailsCommands { Commands = PageHelper.GetMoreCommands(item, null, true) }
        });

        Title = item is Notebook notebook ? notebook.DisplayName : item.Name;
        Command = command;
        Icon = icon ?? Icons.GetIcon(item);
        Tags = tags.ToArray();
        MoreCommands = PageHelper.GetMoreCommands(item, null, page == null).ToContextItems();
        Details = new Details
        {
            Title = item.Name,
            Metadata = metadata.ToArray(),
        };
    }
}