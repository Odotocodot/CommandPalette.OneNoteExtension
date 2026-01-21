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
            ToCommandItem(TopLevelCommandsHelper.DefaultSearch),
            ToCommandItem(TopLevelCommandsHelper.RecentPages),
            ToCommandItem(TopLevelCommandsHelper.OneNoteExplorer),
            ToCommandItem(TopLevelCommandsHelper.QuickNote),
        ];
    }

    private CommandItem ToCommandItem(Page page) => new(page) { Title = page.Title, Subtitle = DisplayName };

    public override void Dispose()
    {
        base.Dispose();
        OneNoteHelper.Dispose();
        GC.SuppressFinalize(this);
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}

