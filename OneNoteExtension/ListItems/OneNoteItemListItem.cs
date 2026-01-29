using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Properties;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace OneNoteExtension.ListItems;

internal partial class OneNoteItemListItem : ListItem
{
    //commandIsOpen -> open in this context means open in oneNote
    public OneNoteItemListItem(IOneNoteItem item, bool addSubtitle, bool commandIsOpen = false) : this(item, Icons.GetIcon(item), addSubtitle, commandIsOpen) {  }
    public OneNoteItemListItem(IOneNoteItem item, IconInfo icon, bool addSubtitle, bool commandIsOpen = false)
    {
        //Tags
        var tags = new List<Tag>();
        if (item.IsUnread)
        {
            tags.Add(new Tag
            {
                Icon = Icons.UnreadChanges,
                ToolTip = Resources.Unread
            });
        }
        if (item.IsInRecycleBin())
        {
            tags.Add(new Tag
            {
                Icon = Icons.RecycleBin,
            });
        }
        var section = item as Section;
        if (section?.Encrypted == true)
        {
            tags.Add(new Tag
            {
                Icon = section.Locked ? Icons.Locked : Icons.Unlocked,
            });
        }

        //Details Body
        StringBuilder sb = new StringBuilder();
        sb.Append("""
            |     |     |
            | :-- | --: |
            """);

        var page = item as LinqToOneNote.Page;
        if (page != null)
        {
            AddProperty(sb, Resources.Created, page.Created);
        }
        AddProperty(sb, Resources.LastModified, item.LastModified);

        if(page == null)
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
            Task.Run(() => Subtitle = OneNoteHelper.GetSubtitle(item, false));
        }

        //Command 
        Command? command = null;
        if (commandIsOpen || page != null || section is { Encrypted: true, Locked: true })
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
            new DetailsElement
            {
                Key = Resources.Commands,
                Data = new DetailsCommands { Commands = OpenInOneNoteCommand.GetAll(item) }
            }
        };
        if (item is IHasPath hasPath)
        {
            metadata.Insert(0, new DetailsElement
            {
                Key = Resources.Hyperlink,
                Data = new DetailsLink(hasPath.Path)
            });
        }

        Title = item is Notebook notebook ? notebook.DisplayName : item.Name;
        Command = command;
        Icon = icon;
        Tags = tags.ToArray();
        Details = new Details
        {
            Title = item.Name,
            Body = sb.ToString(),
            Metadata = metadata.ToArray(),
        };
    }

    private static void AddProperty<T>(StringBuilder sb, string name, T value) => sb.Append(CultureInfo.CurrentCulture, $"\r\n| {name} | {value} |");
}
