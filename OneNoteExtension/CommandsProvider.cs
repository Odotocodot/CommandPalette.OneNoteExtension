using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;

namespace OneNoteExtension;

public partial class CommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public CommandsProvider()
    {
        DisplayName = Resources.DisplayName;
        Icon = Icons.OneNote;
        _commands = [
            ToCommandItem(Helpers.TopLevelCommands.DefaultSearch),
            ToCommandItem(Helpers.TopLevelCommands.RecentPages),
            ToCommandItem(Helpers.TopLevelCommands.OneNoteExplorer),
            ToCommandItem(Helpers.TopLevelCommands.QuickNote),
        ];
    }

    private static CommandItem ToCommandItem(Page page) => page.ToContextItem(Resources.DisplayName);

    public override void Dispose()
    {
        base.Dispose();
        OneNoteHelper.Dispose();
        GC.SuppressFinalize(this);
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}

