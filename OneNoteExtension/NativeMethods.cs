using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace OneNoteExtension;

internal static partial class NativeMethods
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hwnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hwnd, ShowWindowCommand nCmdShow);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FlashWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool bInvert);

    [LibraryImport("user32.dll")]
    private static partial int SendMessage(IntPtr hwnd, int msg, int wParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsIconic(IntPtr hwnd);

    private static class Win32Constants
    {
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu)
        /// or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// Restores the window to its normal position and size.
        /// </summary>
        public const int SC_RESTORE = 0xf120;
    }

    /// <summary>
    /// Show Window Enums
    /// </summary>
    private enum ShowWindowCommand
    {
        /// <summary>
        /// Activates and displays the window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
    }

    public static void BringProcessToFront(string processName)
    {
        var process = Process.GetProcessesByName(processName).FirstOrDefault();
        var mainHwnd = process?.MainWindowHandle;
        if (!mainHwnd.HasValue)
        {
            return;
        }
        var hwnd = mainHwnd.Value;
        if (IsIconic(hwnd) && ShowWindow(hwnd, ShowWindowCommand.Restore))
        {
            _ = SendMessage(hwnd, Win32Constants.WM_SYSCOMMAND, Win32Constants.SC_RESTORE);
        }
        else
        {
            SetForegroundWindow(hwnd);
        }

        FlashWindow(hwnd, true);
    }
}
