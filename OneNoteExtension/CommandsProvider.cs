using System;
using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
using OneNoteExtension.Properties;

namespace OneNoteExtension;

public partial class CommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _topLevelCommands;
    private readonly ICommandItem[] _homePage;
    private readonly SettingsManager _settingsManager = new();

    public CommandsProvider()
    {
        DisplayName = Resources.DisplayName;
        Icon = Icons.Logo;
        Settings = _settingsManager.Settings;

        _settingsManager.Settings.SettingsChanged += OnSettingsChanged;
        IContextItem[] moreCommands = [_settingsManager.Settings.SettingsPage.ToContextItem()];
        _homePage = [new HomePage().ToContextItem(moreCommands)];
        _topLevelCommands = Helpers.TopLevelCommands.Commands.Select(page => page.ToContextItem(moreCommands)).ToArray();
    }

    private void OnSettingsChanged(object sender, Settings args) => RaiseItemsChanged();

    public override ICommandItem[] TopLevelCommands() => _settingsManager.CombineTopLevelCommands ? _homePage : _topLevelCommands;

    public override void Dispose()
    {
        base.Dispose();
        _settingsManager.Settings.SettingsChanged -= OnSettingsChanged;
        OneNoteHelper.Dispose();
        GC.SuppressFinalize(this);
    }
}

