using LinqToOneNote;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OneNoteExtension.Helpers;
using OneNoteExtension.Properties;
using System.ComponentModel;
using System.Diagnostics;
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
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "C:\\Program Files\\Microsoft Office\\Root\\Office16\\ONENOTE.EXE",
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        catch (Win32Exception)
        {
            var mostRecentPage = OneNoteHelper.GetFullHierarchy().Notebooks
                                              .GetAllPages()
                                              .Where(i => !i.IsInRecycleBin)
                                              .OrderByDescending(pg => pg.LastModified)
                                              .First();

            OneNoteHelper.OpenInOneNote(mostRecentPage.Id, false);
        }
        return CommandResult.Dismiss();
    }
}
