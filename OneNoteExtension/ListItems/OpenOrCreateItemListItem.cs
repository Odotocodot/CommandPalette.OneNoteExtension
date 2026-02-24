using LinqToOneNote;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages.Core;
using OneNoteExtension.Properties;
using System.Globalization;
using System.Text;

namespace OneNoteExtension.ListItems;

internal sealed partial class OpenOrCreateItemListItem : OneNoteItemListItem
{
    private static readonly CompositeFormat openXInOneNote = CompositeFormat.Parse(Resources.OpenXInOneNote);
    public OpenOrCreateItemListItem(IExternalItemsChanged listPage, IOneNoteItem item) : base(item, false, true)
    {
        Title = string.Format(CultureInfo.CurrentCulture, openXInOneNote, item.Name);
        Subtitle = Resources.SeeMoreCommands;
        MoreCommands = PageHelper.GetMoreCommands(item, listPage).ToContextItems();
    }
}