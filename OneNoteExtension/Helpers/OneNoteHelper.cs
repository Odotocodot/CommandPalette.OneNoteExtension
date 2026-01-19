using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LinqToOneNote;

namespace OneNoteExtension.Helpers;

// This class used to add a timeout to the OneNote COM object as their is currently no way to know if the Command Palette
// has been closed i.e user is not using the Cmdpal so free up memory by releasing the COM object..
// Every call to LinqToOneNote.OneNote that requires a COM object should come through here. Watch out for extension methods!
internal static class OneNoteHelper
{
    #region Timeout
    private static bool disposedValue;
    private static readonly TimeSpan duration = TimeSpan.FromSeconds(60);
    private static readonly Lazy<Timer> comObjectTimeout = new(
        static () => new Timer(new TimerCallback(TimerCallback),
                               null,
                               duration,
                               Timeout.InfiniteTimeSpan),
        LazyThreadSafetyMode.ExecutionAndPublication
    );
    private static Timer ComObjectTimeout => comObjectTimeout.Value;

    private static void TimerCallback(object? timerState)
    {
        Debug.WriteLine("Releasing COM Object");
        OneNote.ReleaseComObject();
    }

    private static void ResetTimeout() => ComObjectTimeout.Change(duration, Timeout.InfiniteTimeSpan);

    public static void Dispose()
    {
        if (!disposedValue)
        {
            if(comObjectTimeout.IsValueCreated)
            {
                comObjectTimeout.Value.Dispose();
            }
            OneNote.ReleaseComObject();
            disposedValue = true;
        }
    }
    #endregion

    public static void OpenInOneNote(string itemId, bool newWindow)
    {
        ResetTimeout();
        OneNote.Open(itemId, newWindow);
        NativeMethods.BringProcessToFront("onenote");
    }

    public static IEnumerable<Page> FindPages(string search)
    {
        ResetTimeout();
        return OneNote.FindPages(search);
    }

    public static IEnumerable<Page> FindPages(string search, IOneNoteItem scope)
    {
        ResetTimeout();
        return OneNote.FindPages(search, scope);
    }

    public static Root GetFullHierarchy()
    {
        ResetTimeout();
        return OneNote.GetFullHierarchy();
    }

    public static void CreateQuickNote(string? pageName, string? pageContent, bool showOneNote)
    {
        ResetTimeout();
        var pageId = OneNote.CreateQuickNote(pageName, showOneNote ? OpenMode.ExistingOrNewWindow : OpenMode.None);
        OneNote.ComObject.GetPageContent(pageId, out var pageContentXml);
        var xmlWrap = $"""
						<one:Outline>
							<one:Position x="36.0" y="86.4000015258789" z="0"/>
							<one:Size width="72.0" height="13.42771339416504"/>
							<one:OEChildren>
								<one:OE alignment="left">
									<one:T>
										<![CDATA[{pageContent}]]>
									</one:T>
								</one:OE>
							</one:OEChildren>
						</one:Outline>
						""";
        pageContentXml = pageContentXml.Insert(pageContentXml.IndexOf("</one:Page>", StringComparison.Ordinal), xmlWrap);
        OneNote.UpdatePageContent(pageContentXml);
    }
}
