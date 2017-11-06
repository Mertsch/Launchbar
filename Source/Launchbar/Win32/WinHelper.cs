using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace Launchbar.Win32
{
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
        public static BitmapSource ExtractAssociatedIcon([NotNull] string path)
        {
            int i = 0;
            IntPtr hIcon = SafeNativeMethods.ExtractAssociatedIcon(IntPtr.Zero, path, ref i);
            if (hIcon != IntPtr.Zero)
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
        public static BitmapSource ExtractIcon([NotNull] string path, int index)
        {
            IntPtr hIcon = SafeNativeMethods.ExtractIcon(IntPtr.Zero, path, (uint)index);
            if (hIcon != IntPtr.Zero)
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
            SafeNativeMethods.MOUSEINPUT mi = new SafeNativeMethods.MOUSEINPUT();

            switch (button)
            {
                case MouseButton.Left:
                    mi.dwFlags = SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTDOWN;
                    break;
                case MouseButton.Right:
                    mi.dwFlags = SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTDOWN;
                    break;
                default:
                    throw new NotSupportedException();
            }
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
            SafeNativeMethods.MOUSEINPUT mi = new SafeNativeMethods.MOUSEINPUT();

            switch (button)
            {
                case MouseButton.Left:
                    mi.dwFlags = SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTUP;
                    break;
                case MouseButton.Right:
                    mi.dwFlags = SafeNativeMethods.MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTUP;
                    break;
                default:
                    throw new NotSupportedException();
            }

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
                    dy = y
                };

            SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT mkhInput = new SafeNativeMethods.MOUSEKEYBDHARDWAREINPUT
                {
                    mi = mi
                };

            SafeNativeMethods.INPUT input = new SafeNativeMethods.INPUT
                {
                    type = SafeNativeMethods.INPUT_TYPE.MOUSE,
                    mkhi = mkhInput
                };

            SafeNativeMethods.SendInput(1, ref input, Marshal.SizeOf(input));
        }

        public static void SetAsToolWindow([NotNull] this Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).EnsureHandle();
            IntPtr oldFlags = SafeNativeMethods.GetWindowLongPtr(handle, GWL.GWL_EXSTYLE);
            if (oldFlags != IntPtr.Zero)
            {
                IntPtr newFlags = new IntPtr(oldFlags.ToInt64() | ExtendedWindowStyles.WS_EX_TOOLWINDOW);
                SafeNativeMethods.SetWindowLongPtr(handle, GWL.GWL_EXSTYLE, newFlags);
            }
        }
    }
}