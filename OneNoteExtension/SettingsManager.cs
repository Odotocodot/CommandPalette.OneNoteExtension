using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Properties;

namespace OneNoteExtension;

internal sealed class SettingsManager : JsonSettingsManager
{
    private const string _namespace = "onenote";
    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";

    public bool CombineTopLevelCommands => _combineTopLevelCommands.Value;

    private readonly ToggleSetting _combineTopLevelCommands = new
    (
        Namespaced(nameof(CombineTopLevelCommands)),
        Resources.Settings_CombineTopLevelCommands,
        string.Empty,
        false
    );

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

        LoadSettings();

        Settings.SettingsChanged += (_, _) => SaveSettings();
    }
}