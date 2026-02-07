using LinqToOneNote;
using LinqToOneNote.Abstractions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Commands;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using OneNoteExtension.Properties;

namespace OneNoteExtension.ListItems;

internal partial class OpenOrCreateItemListItem : OneNoteItemListItem
{
    private static readonly CompositeFormat openXInOneNote = CompositeFormat.Parse(Resources.OpenXInOneNote);
    public OpenOrCreateItemListItem(IOneNoteItem item) : base(item, false, true)
    {
        Title = string.Format(CultureInfo.CurrentCulture, openXInOneNote, item.Name);
        Subtitle = Resources.SeeMoreCommands;
        MoreCommands = PageHelper.GetMoreCommands(item).ToArray();
    }
}