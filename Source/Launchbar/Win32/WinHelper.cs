using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;
using INPUT_TYPE_UNION = Windows.Win32.UI.Input.KeyboardAndMouse.INPUT._Anonymous_e__Union;

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
        ushort iconIndex = 0;
        Span<char> pathBuffer = new char[path.Length + 1];
        path.CopyTo(pathBuffer);
        using DestroyIconSafeHandle iconHandle = PInvoke.ExtractAssociatedIcon(ref pathBuffer, ref iconIndex);
        if (iconHandle.IsInvalid)
        {
            return null;
        }
        return Imaging.CreateBitmapSourceFromHIcon(iconHandle.DangerousGetHandle(), Int32Rect.Empty, null);
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
        using DestroyIconSafeHandle iconHandle = PInvoke.ExtractIcon(path, (uint)index);
        if (iconHandle.IsInvalid)
        {
            return null;
        }
        return Imaging.CreateBitmapSourceFromHIcon(iconHandle.DangerousGetHandle(), Int32Rect.Empty, null);
    }

    /// <summary>
    /// Send a mouse button down signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static unsafe void SendMouseButtonDown(MouseButton button)
    {
        INPUT input = new INPUT
            {
                type = INPUT_TYPE.INPUT_MOUSE,
                Anonymous = new INPUT_TYPE_UNION
                    {
                        mi = new MOUSEINPUT
                            {
                                dwFlags = button switch
                                    {
                                        MouseButton.Left => MOUSE_EVENT_FLAGS.MOUSEEVENTF_LEFTDOWN,
                                        MouseButton.Right => MOUSE_EVENT_FLAGS.MOUSEEVENTF_RIGHTDOWN,
                                        _ => throw new NotSupportedException(),
                                    },
                            },
                    },
            };

        PInvoke.SendInput([input], sizeof(INPUT));
    }

    /// <summary>
    /// Send a mouse button up signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static unsafe void SendMouseButtonUp(MouseButton button)
    {
        INPUT input = new INPUT
            {
                type = INPUT_TYPE.INPUT_MOUSE,
                Anonymous = new INPUT_TYPE_UNION
                    {
                        mi = new MOUSEINPUT
                            {
                                dwFlags = button switch
                                    {
                                        MouseButton.Left => MOUSE_EVENT_FLAGS.MOUSEEVENTF_LEFTUP,
                                        MouseButton.Right => MOUSE_EVENT_FLAGS.MOUSEEVENTF_RIGHTUP,
                                        _ => throw new NotSupportedException(),
                                    },
                            },
                    },
            };

        PInvoke.SendInput([input], sizeof(INPUT));
    }

    /// <summary>
    /// Send a mouse move signal.
    /// <remarks>Errors will not be handled.</remarks>
    /// </summary>
    public static unsafe void SendMouseMoveRelative(int x, int y)
    {
        INPUT input = new INPUT
            {
                type = INPUT_TYPE.INPUT_MOUSE,
                Anonymous = new INPUT_TYPE_UNION
                    {
                        mi = new MOUSEINPUT
                            {
                                dwFlags = MOUSE_EVENT_FLAGS.MOUSEEVENTF_MOVE,
                                dx = x,
                                dy = y,
                            },
                    },
            };

        PInvoke.SendInput([input], sizeof(INPUT));
    }

    public static void SetAsToolWindow(this Window window)
    {
        nint handle = new WindowInteropHelper(window).EnsureHandle();
        nint oldFlags = PInvoke.GetWindowLongPtr(new HWND(handle), WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        if (oldFlags != nint.Zero)
        {
            nint newFlags = new nint(oldFlags | (long)WINDOW_EX_STYLE.WS_EX_TOOLWINDOW);
            PInvoke.SetWindowLongPtr(new HWND(handle), WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, newFlags);
        }
    }

    public static bool PickIconDialog(ref string path, ref int index)
    {
        // We need to have a string that is long enough to handle a more complex path than the default one (more characters in length).
        Span<char> pathBuffer = new char[32 * 1024]; // https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation
        path.CopyTo(pathBuffer); // We assume that 'path' is shorter than 'pathBuffer' and thus we have a null terminated buffer.

        // https://learn.microsoft.com/en-us/windows/win32/api/shlobj_core/nf-shlobj_core-pickicondlg#return-value
        const int Success = 1;
        if (PInvoke.PickIconDlg(HWND.Null, ref pathBuffer, (uint)pathBuffer.Length, ref index) is Success)
        {
            path = pathBuffer.ToString(); // The friendly helper already limits the buffer to the strings length.
            return true;
        }
        return false;
    }
}