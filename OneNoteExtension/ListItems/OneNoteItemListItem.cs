using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;

namespace OneNoteExtension.ListItems;

internal partial class OneNoteItemListItem : ListItem
{
    private static readonly CompositeFormat openXInOneNote = CompositeFormat.Parse(Resources.OpenXInOneNote);
    public OneNoteItemListItem(IExternalItemsChanged listPage, IOneNoteItem item) : this(item, false, true)
    {
        Title = string.Format(CultureInfo.CurrentCulture, openXInOneNote, item.Name);
        Subtitle = Resources.SeeMoreCommands;
        MoreCommands = PageHelper.GetMoreCommands(item, listPage).ToContextItems();
    }

    public OneNoteItemListItem(IOneNoteItem item, bool addSubtitle, bool commandIsOpenInOneNote, IconInfo? icon = null)
    {
        //Tags
        var tags = new List<Tag>();
        if (item.IsUnread)
        {
            tags.Add(new Tag
            {
                Icon = Icons.UnreadChanges,
                ToolTip = Resources.UnreadToolTip
            });
        }
        if (item.IsInRecycleBin())
        {
            tags.Add(new Tag
            {
                Icon = Icons.RecycleBin,
                ToolTip = Resources.RecycleBinToolTip
            });
        }
        var section = item as Section;
        if (section?.Encrypted == true)
        {
            tags.Add(section.Locked
                ? new Tag { Icon = Icons.Locked, ToolTip = Resources.LockedToolTip }
                : new Tag { Icon = Icons.Unlocked, ToolTip = Resources.UnlockedToolTip }
                );
        }

        //Details Body
        var sb = new StringBuilder();
        sb.Append(CultureInfo.CurrentCulture, $"""
            | {Resources.Property} | {Resources.Value} |
            | :-- | --: |
            """);

        var page = item as LinqToOneNote.Page;
        if (page != null)
        {
            AddProperty(sb, Resources.Created, page.Created);
        }
        AddProperty(sb, Resources.LastModified, item.LastModified);

        if (page == null)
        {
            if (item is INotebookOrSectionGroup notebookOrSectionGroup)
            {
                AddProperty(sb, Resources.Sections, notebookOrSectionGroup.Sections.Count);
                AddProperty(sb, Resources.SectionGroups, notebookOrSectionGroup.SectionGroups.Count);
                //AddProperty(sb, Resources.TotalPages, notebookOrSectionGroup.Children.GetAllPages().Count());
            }
            if (section != null)
            {
                AddProperty(sb, Resources.Pages, section.Pages.Count);
            }
        }

        //Subtitle
        if (addSubtitle)
        {
            Task.Run(() => Subtitle = PageHelper.GetSubtitle(item, false));
        }

        //Command 
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
        var metadata = new List<DetailsElement>
        {
            new()
            {
                Key = Resources.Commands,
                Data = new DetailsCommands { Commands = PageHelper.GetMoreCommands(item, null, true) }
            }
        };
        if (item is IHasPath hasPath)
        {
            metadata.Insert(0, new()
            {
                Key = Resources.Path,
                Data = new DetailsLink(hasPath.Path)
            });
        }
        if (tags.Count > 0)
        {
            metadata.Add(new()
            {
                Key = Resources.Tags,
                Data = new DetailsTags { Tags = tags.ToArray() }
            });
        }

        Title = item is Notebook notebook ? notebook.DisplayName : item.Name;
        Command = command;
        Icon = icon ?? Icons.GetIcon(item);
        Tags = tags.ToArray();
        MoreCommands = PageHelper.GetMoreCommands(item, null, page == null).ToContextItems();
        Details = new Details
        {
            Title = item.Name,
            Body = sb.ToString(),
            Metadata = metadata.ToArray(),
        };
    }
    private static void AddProperty<T>(StringBuilder sb, string name, T value) => sb.Append(CultureInfo.CurrentCulture, $"\r\n| {name} | {value} |");
}