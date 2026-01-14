// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OneNoteExtension;

internal static class Icons
{
    private static bool coloredIcons = true;
    public static IconInfo Search => FromAssetName("search");
    public static IconInfo OneNote => IconHelpers.FromRelativePath("Assets\\onenote.svg");
    public static IconInfo Invalid => IconHelpers.FromRelativePath("Assets\\warning.light.svg");
    public static IconInfo Page => FromAssetName("page");
    public static IconInfo OpenInNewWindow { get; } = new IconInfo("\ue8a7");
    public static IconInfo Open { get; } = new IconInfo("\ue8e5");

    public static IconInfo FromAssetName(string assetName) => coloredIcons
        ? IconHelpers.FromRelativePath($"Assets\\{assetName}.color.svg")
        : IconHelpers.FromRelativePath($"Assets\\{assetName}.color.svg");
}