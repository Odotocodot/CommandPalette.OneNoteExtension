// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Pages;
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
            new DefaultSearchPage().ToCommandItem(DisplayName),
            new RecentItemsPage().ToCommandItem(DisplayName),
            new OneNoteExplorerRootPage().ToCommandItem(DisplayName),
        ];
    }

    public override void Dispose()
    {
        base.Dispose();
        OneNote.ReleaseComObject();
        GC.SuppressFinalize(this);
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}