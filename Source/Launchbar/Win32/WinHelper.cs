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
        nint hIcon = SafeNativeMethods.ExtractAssociatedIcon(nint.Zero, path, ref i);
        if (hIcon != nint.Zero)
        {
            BitmapSource bms = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, null);
            SafeNativeMethods.DestroyIcon(hIcon);
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
        nint hIcon = SafeNativeMethods.ExtractIcon(nint.Zero, path, (uint)index);
        if (hIcon != nint.Zero)
        {
            BitmapSource bms = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, null);
            SafeNativeMethods.DestroyIcon(hIcon);
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
        SafeNativeMethods.MOUSEINPUT mi = new SafeNativeMethods.MOUSEINPUT
            {
                dwFlags = button switch
                    {
                        MouseButton.Left => SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTDOWN,
                        MouseButton.Right => SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTDOWN,
                        _ => throw new NotSupportedException(),
                    },
            };

        SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT mkhInput = new SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT { mi = mi };

        SafeNativeMethods.INPUT input = new SafeNativeMethods.INPUT { type = SafeNativeMethods.INPUT_TYPE.MOUSE, mkhi = mkhInput };

        SafeNativeMethods.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    /// <summary>
    /// Send a mouse button up signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static void SendMouseButtonUp(MouseButton button)
    {
        SafeNativeMethods.MOUSEINPUT mi = new SafeNativeMethods.MOUSEINPUT
            {
                dwFlags = button switch
                    {
                        MouseButton.Left => SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTUP,
                        MouseButton.Right => SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTUP,
                        _ => throw new NotSupportedException(),
                    },
            };

        SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT mkhInput = new SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT { mi = mi };

        SafeNativeMethods.INPUT input = new SafeNativeMethods.INPUT { type = SafeNativeMethods.INPUT_TYPE.MOUSE, mkhi = mkhInput };

        SafeNativeMethods.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    /// <summary>
    /// Send a mouse move signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static void SendMouseMoveRelative(int x, int y)
    {
        SafeNativeMethods.MOUSEINPUT mi = new SafeNativeMethods.MOUSEINPUT
            {
                dwFlags = SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_MOVE,
                dx = x,
                dy = y,
            };

        SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT mkhInput = new SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT
            {
                mi = mi,
            };

        SafeNativeMethods.INPUT input = new SafeNativeMethods.INPUT
            {
                type = SafeNativeMethods.INPUT_TYPE.MOUSE,
                mkhi = mkhInput,
            };

        SafeNativeMethods.SendInput(1, ref input, Marshal.SizeOf(input));
    }

    public static void SetAsToolWindow(this Window window)
    {
        nint handle = new WindowInteropHelper(window).EnsureHandle();
        nint oldFlags = SafeNativeMethods.GetWindowLongPtr(handle, GWL.GWL_EXSTYLE);
        if (oldFlags != nint.Zero)
        {
            nint newFlags = new nint(oldFlags.ToInt64() | ExtendedWindowStyles.WS_EX_TOOLWINDOW);
            SafeNativeMethods.SetWindowLongPtr(handle, GWL.GWL_EXSTYLE, newFlags);
        }
    }
}