using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Launchbar.Win32;

/// <summary>
/// Contains various methods to work with the windows operating system.
/// </summary>
internal static class WinHelper
{
    /// <summary>
    /// Extract the associated icon from a file or directory.
    /// </summary>
    /// <param name="path">Path to the file or directory to extract the icon from.
    /// <remarks>Does NOT support environment variables.</remarks></param>
    /// <returns>The extracted icon as <see cref="BitmapSource"/>.</returns>
    public static BitmapSource? ExtractAssociatedIcon(string path)
    {
        int i = 0;
        nint hIcon = LibraryImport.ExtractAssociatedIcon(nint.Zero, path, ref i);
        if (hIcon != nint.Zero)
        {
            BitmapSource bms = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, null);
            LibraryImport.DestroyIcon(hIcon);
            return bms;
        }
        return null;
    }

    /// <summary>
    /// Extract an icon from a file or directory.
    /// </summary>
    /// <param name="path">Path to the file or directory to extract the icon from.
    /// <remarks>Supports environment variables.</remarks></param>
    /// <param name="index">A zero-based index of the icon.</param>
    /// <returns>The extracted icon as <see cref="BitmapSource"/>.</returns>
    public static BitmapSource? ExtractIcon(string path, int index)
    {
        nint hIcon = LibraryImport.ExtractIcon(nint.Zero, path, (uint)index);
        if (hIcon != nint.Zero)
        {
            BitmapSource bms = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, null);
            LibraryImport.DestroyIcon(hIcon);
            return bms;
        }
        return null;
    }

    /// <summary>
    /// Send a mouse button down signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static void SendMouseButtonDown(MouseButton button)
    {
        LibraryImport.MOUSEINPUT mi = new LibraryImport.MOUSEINPUT
            {
                dwFlags = button switch
                    {
                        MouseButton.Left => LibraryImport.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTDOWN,
                        MouseButton.Right => LibraryImport.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTDOWN,
                        _ => throw new NotSupportedException(),
                    },
            };

        LibraryImport.MOUSEKEYBDHARDWAREINPUT mkhInput = new LibraryImport.MOUSEKEYBDHARDWAREINPUT { mi = mi };

        LibraryImport.INPUT input = new LibraryImport.INPUT { type = LibraryImport.INPUT_TYPE.MOUSE, mkhi = mkhInput };

        LibraryImport.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    /// <summary>
    /// Send a mouse button up signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static void SendMouseButtonUp(MouseButton button)
    {
        LibraryImport.MOUSEINPUT mi = new LibraryImport.MOUSEINPUT
            {
                dwFlags = button switch
                    {
                        MouseButton.Left => LibraryImport.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTUP,
                        MouseButton.Right => LibraryImport.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTUP,
                        _ => throw new NotSupportedException(),
                    },
            };

        LibraryImport.MOUSEKEYBDHARDWAREINPUT mkhInput = new LibraryImport.MOUSEKEYBDHARDWAREINPUT { mi = mi };

        LibraryImport.INPUT input = new LibraryImport.INPUT { type = LibraryImport.INPUT_TYPE.MOUSE, mkhi = mkhInput };

        LibraryImport.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    /// <summary>
    /// Send a mouse move signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static void SendMouseMoveRelative(int x, int y)
    {
        LibraryImport.MOUSEINPUT mi = new LibraryImport.MOUSEINPUT
            {
                dwFlags = LibraryImport.MOUSEINPUTFLAGS.MOUSEEVENTF_MOVE,
                dx = x,
                dy = y,
            };

        LibraryImport.MOUSEKEYBDHARDWAREINPUT mkhInput = new LibraryImport.MOUSEKEYBDHARDWAREINPUT
            {
                mi = mi,
            };

        LibraryImport.INPUT input = new LibraryImport.INPUT
            {
                type = LibraryImport.INPUT_TYPE.MOUSE,
                mkhi = mkhInput,
            };

        LibraryImport.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    public static void SetAsToolWindow(this Window window)
    {
        nint handle = new WindowInteropHelper(window).EnsureHandle();
        nint oldFlags = LibraryImport.GetWindowLongPtr(handle, GWL.GWL_EXSTYLE);
        if (oldFlags != nint.Zero)
        {
            nint newFlags = new nint(oldFlags.ToInt64() | ExtendedWindowStyles.WS_EX_TOOLWINDOW);
            LibraryImport.SetWindowLongPtr(handle, GWL.GWL_EXSTYLE, newFlags);
        }
    }

    public static bool PickIconDialog(ref string path, ref int index)
    {
        // We need to have a string that is long enough to handle a more complex path than the default one (more characters in length).
        char[] pathBuffer = new char[32 * 1024]; // https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation
        path.CopyTo(pathBuffer);

        // Method returns one when pressing OK in the dialog.
        if (LibraryImport.PickIconDlg(nint.Zero, pathBuffer, (uint)pathBuffer.Length, ref index) == 1)
        {
            path = new string(pathBuffer, 0, Array.IndexOf(pathBuffer, '\0')); // Extract string from null terminated string.
            return true;
        }
        return false;
    }
}