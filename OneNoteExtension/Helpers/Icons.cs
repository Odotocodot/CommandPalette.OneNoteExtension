using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension.Helpers;

internal static class Icons
{
    private const string _path = "Assets\\Icons";
    public static IconInfo Logo { get; } = IconHelpers.FromRelativePath($"{_path}\\logo.svg");
    public static IconInfo OneNote { get; } = IconHelpers.FromRelativePath($"{_path}\\onenote.svg");
    public static IconInfo Invalid { get; } = IconHelpers.FromRelativePath($"{_path}\\warning.light.svg");
    public static IconInfo Open { get; } = new("\ue8a7"); //OpenInNewWindow
    public static IconInfo RecentPage { get; } = FromAssetName("page_recent");
    public static IconInfo OneNoteExplorer { get; } = FromAssetName("notebook_explorer");
    public static IconInfo Page { get; } = FromAssetName("page");
    public static IconInfo RecycleBin { get; } = FromAssetName("recycle_bin");
    public static IconInfo NewPage { get; } = FromAssetName("page_new");
    public static IconInfo NewSection { get; } = FromAssetName("section_new");
    public static IconInfo NewSectionGroup { get; } = FromAssetName("section_group_new");
    public static IconInfo NewNotebook { get; } = FromAssetName("notebook_new");
    public static IconInfo UnreadChanges { get; } = new("\ue70f"); //Edit
    public static IconInfo Locked { get; } = new("\ue72e"); //Lock
    public static IconInfo Unlocked { get; } = new("\ue785"); //Unlock
    public static IconInfo Title { get; } = new("\uf714"); //RTTLogo
    public static IconInfo SearchPages { get; } = FromAssetName("page_search");

    private static IconInfo FromAssetName(string assetName) => IconHelpers.FromRelativePath($"{_path}\\{assetName}.color.svg");

    public static IconInfo GetIcon(IOneNoteItem item) => item switch
    {
        Notebook nb => FromAssetOrCreate("notebook", nb.Color),
        SectionGroup sg => sg.IsRecycleBin ? FromAssetName("recycle_bin") : FromAssetName("section_group"),
        Section s => FromAssetOrCreate("section", s.Color),
        LinqToOneNote.Page => Page,
        _ => Logo,
    };

    private static IconInfo FromAssetOrCreate(string assetName, Color? color)
    {
        if (!color.HasValue)
        {
            return IconHelpers.FromRelativePath($"{_path}\\{assetName}.color.svg");
        }

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var colorString = ColorTranslator.ToHtml(color.Value);

        var file = Path.Combine(baseDir, _path, "Generated", $"{assetName}.{colorString[1..]}.svg");
        if (File.Exists(file))
        {
            return new IconInfo(file);
        }

        var baseImage = Path.Combine(baseDir, _path, $"{assetName}.dark.svg");
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