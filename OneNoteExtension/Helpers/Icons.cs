using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Helpers;

internal static class Icons
{
    public static IconInfo Search => FromAssetName("search");
    public static IconInfo OneNote => IconHelpers.FromRelativePath("Assets\\onenote.svg");
    public static IconInfo Invalid => IconHelpers.FromRelativePath("Assets\\warning.light.svg");
    public static IconInfo OpenInNewWindow { get; } = new IconInfo("\ue8a7");
    public static IconInfo Open { get; } = new IconInfo("\ue8e5");
    public static IconInfo RecentPage => FromAssetName("page_recent");
    public static IconInfo OneNoteExplorer => FromAssetName("notebook_explorer");
    public static IconInfo Page => FromAssetName("page");
    public static IconInfo RecycleBin => FromAssetName("recycle_bin");

    private static IconInfo FromAssetName(string assetName) => IconHelpers.FromRelativePath($"Assets\\{assetName}.color.svg");

    public static IconInfo GetIcon(IOneNoteItem item) => item switch
    {
        Notebook nb => FromAssetOrCreate("notebook", nb.Color),
        SectionGroup sg => sg.IsRecycleBin ? FromAssetName("recycle_bin") : FromAssetName("section_group"),
        Section s => FromAssetOrCreate("section", s.Color),
        LinqToOneNote.Page => Page,
        _ => OneNote,
    };

    private static IconInfo FromAssetOrCreate(string assetName, Color? color)
    {
        if (!color.HasValue)
        {
            return IconHelpers.FromRelativePath($"Assets\\{assetName}.color.svg");
        }

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var colorString = ColorTranslator.ToHtml(color.Value);

        var file = Path.Combine(baseDir, "Assets", "Generated", $"{assetName}.{colorString[1..]}.svg");
        if (File.Exists(file))
        {
            return new IconInfo(file);
        }

        var baseImage = Path.Combine(baseDir, "Assets", $"{assetName}.dark.svg");
        var root = XElement.Load(baseImage);

        const string SvgNamespace = "http://www.w3.org/2000/svg";
        root.Element(XName.Get("defs", SvgNamespace))!
            .Element(XName.Get("linearGradient", SvgNamespace))!
            .Element(XName.Get("stop", SvgNamespace))!
            .Attribute("stop-color")!.Value = colorString;

        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        root.Save(file, SaveOptions.DisableFormatting);

        return new IconInfo(file);
    }
}