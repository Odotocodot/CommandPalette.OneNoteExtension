using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Properties;

namespace OneNoteExtension;

internal sealed class SettingsManager : JsonSettingsManager
{
    private const string _namespace = "onenote";
    private static readonly Lazy<SettingsManager> instance = new(() => new SettingsManager(), LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly ToggleSetting _combineTopLevelCommands = new
    (
        GetKey(),
        Resources.Settings_CombineTopLevelCommands,
        string.Empty,
        false
    );

    private readonly ToggleSetting _showRecycleBinEntries = new
    (
        GetKey(),
        Resources.Settings_ShowRecycleBinEntries,
        string.Empty,
        true
    );

    public static SettingsManager Instance => instance.Value;

    public bool CombineTopLevelCommands => _combineTopLevelCommands.Value;
    public bool ShowRecycleBinEntries => _showRecycleBinEntries.Value;

    private static string GetKey([CallerMemberName] string propertyName = "") => $"{_namespace}.{propertyName.TrimStart('_')}";

    private static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("OneNoteExtension");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }

    public SettingsManager()
    {
        FilePath = SettingsJsonPath();

        Settings.Add(_combineTopLevelCommands);
        Settings.Add(_showRecycleBinEntries);

        LoadSettings();

        Settings.SettingsChanged += (_, _) => SaveSettings();
    }
}