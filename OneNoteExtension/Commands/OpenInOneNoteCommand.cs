// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Properties;
namespace OneNoteExtension.Commands;

internal partial class OpenInOneNoteCommand : InvokableCommand
{
    private readonly IOneNoteItem _item;
    private readonly bool _newWindow;

    public OpenInOneNoteCommand(IOneNoteItem item, bool newWindow = false)
    {
        _item = item;
        _newWindow = newWindow;
        Name = newWindow ? Resources.OpenInNewWindow : Resources.Open;
        //Icon = newWindow ? Icons.OpenInNewWindow : Icons.Open;
    }

    public override ICommandResult Invoke()
    {
        OneNote.Open(_item, _newWindow);
        NativeMethods.BringProcessToFront("onenote");
        return CommandResult.Dismiss();
    }

    public static ICommand[] GetAll(IOneNoteItem item)
    {
        return
        [
            new OpenInOneNoteCommand(item, false),
            new OpenInOneNoteCommand(item, true)
        ];
    }
}
