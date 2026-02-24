using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Pages;

internal sealed partial class HomePage : ListPage
{
    public HomePage()
    {
        Name = Title = Resources.DisplayName;
        Icon = Icons.Logo;
    }

    public override IListItem[] GetItems() => Array.ConvertAll(TopLevelCommands.Commands,static p => new ListItem(p));
}