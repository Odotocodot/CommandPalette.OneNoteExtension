using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;

namespace OneNoteExtension.Commands;

internal partial class CopyLinkToClipboardCommand : InvokableCommand
{
    private readonly IOneNoteItem _item;

    public CopyLinkToClipboardCommand(IOneNoteItem item)
    {
        _item = item;
        Name = Resources.CopyLinkToClipboard;
        Icon = Icons.Copy;
    }

    public override ICommandResult Invoke()
    {
        ClipboardHelper.SetText(OneNoteHelper.GetHyperlink(_item));
        return CommandResult.ShowToast(Resources.CopiedLinkToClipboard);
    }
}
