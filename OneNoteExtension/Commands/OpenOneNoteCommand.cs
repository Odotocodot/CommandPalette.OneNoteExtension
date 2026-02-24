using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;
using System.Linq;

namespace OneNoteExtension.Commands;

internal sealed partial class OpenOneNoteCommand : InvokableCommand
{
    public OpenOneNoteCommand()
    {
        Icon = Icons.OneNote;
        Name = Resources.OpenOneNote;
    }

    public override ICommandResult Invoke()
    {
        var mostRecentPage = OneNoteHelper.GetFullHierarchy().Notebooks
                                          .GetAllPages()
                                          .Where(i => !i.IsInRecycleBin)
                                          .OrderByDescending(pg => pg.LastModified)
                                          .First();

        OneNoteHelper.OpenInOneNote(mostRecentPage.Id, false);
        return CommandResult.Dismiss();
        // Below currently doesn't with Command Palette, creates a "We're sorry. OneNote is cleaning up from the last time it was open. Please Wait." Dialog box.
        // Works outside of Command Palette though.
        //
        //try
        //{
        //    var startInfo = new ProcessStartInfo
        //    {
        //        FileName = "C:\\Program Files\\Microsoft Office\\Root\\Office16\\ONENOTE.EXE",
        //        UseShellExecute = true,
        //        CreateNoWindow = true,
        //    };
        //    using var process = new Process();
        //    process.StartInfo = startInfo;
        //    process.Start();

        //}
        //catch (Win32Exception)
        //{
        //}
        //return CommandResult.Dismiss();
    }
}
