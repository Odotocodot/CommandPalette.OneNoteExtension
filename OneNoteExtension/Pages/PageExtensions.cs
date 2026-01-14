// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions.Toolkit;
namespace OneNoteExtension.Pages;

internal static class PageExtensions
{
    public static CommandItem ToCommandItem(this Page page, string subtitle = "") => new(page)
    {
        Title = page.Title,
        Subtitle = subtitle
    };
}
