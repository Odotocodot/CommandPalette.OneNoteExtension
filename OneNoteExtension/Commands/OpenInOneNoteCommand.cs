using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
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
        Icon = Icons.Open;
    }

    public override ICommandResult Invoke()
    {
        OneNoteHelper.OpenInOneNote(_item.Id, _newWindow);
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
