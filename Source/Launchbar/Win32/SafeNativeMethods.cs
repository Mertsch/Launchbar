using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Launchbar.Win32
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        internal const string User32 = @"user32.dll";

        internal const string Shell32 = @"shell32.dll";

        #region SetWindowLongPtr

        [DllImport(User32, EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport(User32, EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        /// <summary>
        /// The GetWindowLongPtr function retrieves information about the specified window.
        /// The function also retrieves the value at a specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved.
        /// Valid values are in the range zero through the number of bytes of extra window memory,
        /// minus the size of an integer.</param>
        /// <returns>If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// If SetWindowLong or SetWindowLongPtr has not been called previously, GetWindowLongPtr returns zero
        /// for values in the extra window or class memory.</returns>
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : new IntPtr(GetWindowLongPtr32(hWnd, nIndex));
        }

        #endregion

        #region SetWindowLongPtr

        [DllImport(User32, EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(User32, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// The SetWindowLongPtr function changes an attribute of the specified window.
        /// The function also sets a value at the specified offset in the extra window memory.
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.
        /// The SetWindowLongPtr function fails if the process that owns the window specified by the
        /// hWnd parameter is at a higher process privilege in the User Interface Privilege Isolation (UIPI)
        /// hierarchy than the process the calling thread resides in. Microsoft Windows XP and earlier:
        /// The SetWindowLongPtr function fails if the window specified by the hWnd parameter does not belong
        /// to the same process as the calling thread.</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be set.
        /// Valid values are in the range zero through the number of bytes of extra window memory,
        /// minus the size of an integer.</param>
        /// <param name="dwNewLong">Specifies the replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified offset.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// If the previous value is zero and the function succeeds, the return value is zero, but the function
        /// does not clear the last error information. To determine success or failure, clear the last
        /// error information by calling SetLastError(0), then call SetWindowLongPtr.
        /// Function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</returns>
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return IntPtr.Size == 8
                ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
                : new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        #endregion

        /// <summary>
        /// Displays a dialog box that enables a user to select an icon from a module.
        /// </summary>
        /// <param name="hwnd">[in] The handle of the parent window.</param>
        /// <param name="pszIconPath">[in, out] A null-terminated string that contains the fully-qualified path
        /// of the default DLL that contains the icons. If the user chooses a different DLL, this buffer
        /// contains the path of that DLL when the function returns. This buffer should be at least MAX_PATH
        /// characters in length, or the returned path may be truncated. You should verify that the path is
        /// valid before using it.</param>
        /// <param name="cchIconPath">[in] The number of characters in <paramref name="pszIconPath"/>,
        /// including the terminating NULL character.</param>
        /// <param name="piIconIndex">[in, out] A pointer to an integer that, on entry, specified the index of
        /// the initial selection. On exit, the integer specifies the index of the icon that was selected.</param>
        /// <returns>Returns 1 if successful; otherwise, 0.</returns>
        [DllImport(Shell32, EntryPoint = "PickIconDlg", CharSet = CharSet.Unicode)]
        public static extern int PickIconDlg(IntPtr hwnd, StringBuilder pszIconPath, uint cchIconPath, ref int piIconIndex);

        /// <summary>
        /// Destroys an icon and frees any memory the icon occupied.
        /// </summary>
        /// <param name="hIcon">[in] Handle to the icon to be destroyed. The icon must not be in use.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails,
        /// the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport(User32, EntryPoint = "DestroyIcon", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// The ExtractIcon function retrieves a handle to an icon from the specified executable file, DLL, or icon file.
        /// </summary>
        /// <param name="hInst">[in] Handle to the instance of the application calling the function.</param>
        /// <param name="lpszExeFileName">[in] Pointer to a null-terminated string specifying the name of an
        /// executable file, DLL, or icon file.</param>
        /// <param name="nIconIndex">[in] Specifies the zero-based index of the icon to retrieve.
        /// For example, if this value is 0, the function returns a handle to the first icon in the specified file.
        /// If this value is -1, the function returns the total number of icons in the specified file.
        /// If the file is an executable file or DLL, the return value is the number of RT_GROUP_ICON resources.
        /// If the file is an .ICO file, the return value is 1.</param>
        /// <returns>The return value is a handle to an icon. If the file specified was not an executable file, DLL, or icon file, the return is 1.
        /// If no icons were found in the file, the return value is NULL.</returns>
        [DllImport(Shell32, EntryPoint = "ExtractIcon", CharSet = CharSet.Unicode)]
        internal static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, uint nIconIndex);

        /// <summary>
        /// The ExtractAssociatedIcon function returns a handle to an indexed icon found in a file or an icon found in an associated executable file.
        /// </summary>
        /// <param name="hInst">[in] Specifies the instance of the application calling the function.</param>
        /// <param name="lpIconPath">[in] Pointer to a string that specifies the full path and file name of the
        /// file that contains the icon. The function extracts the icon handle from that file, or from an
        /// executable file associated with that file. If the icon handle is obtained from an executable file,
        /// the function stores the full path and file name of that executable in the string pointed to by
        /// <paramref name="lpIconPath"/>.</param>
        /// <param name="lpiIcon">[in, out] Pointer to a WORD that specifies the index of the icon whose handle
        /// is to be obtained. If the icon handle is obtained from an executable file, the function stores the
        /// icon's identifier in the WORD pointed to by <paramref name="lpiIcon"/>.</param>
        /// <returns>If the function succeeds, the return value is an icon handle. If the icon is extracted
        /// from an associated executable file, the function stores the full path and file name of the
        /// executable file in the string pointed to by <paramref name="lpIconPath"/>, and stores the icon's
        /// identifier in the WORD pointed to by <paramref name="lpiIcon"/>. If the function fails, the
        /// return value is NULL.</returns>
        [DllImport(Shell32, EntryPoint = "ExtractAssociatedIcon", CharSet = CharSet.Unicode)]
        internal static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, string lpIconPath, ref int lpiIcon);

        #region SendInput

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int dx;

            public int dy;

            public MOUSEINPUTDATA mouseData;

            public MOUSEINPUTFLAGS dwFlags;

            public uint time;

            public IntPtr dwExtraInfo;
        }

        internal enum MOUSEINPUTDATA : uint
        {
            XBUTTON1 = 0x0001,
            XBUTTON2 = 0x0002,
        }

        [Flags]
        internal enum MOUSEINPUTFLAGS : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort wVk;

            public ushort wScan;

            public uint dwFlags;

            public uint time;

            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public int uMsg;

            public short wParamL;

            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        internal enum INPUT_TYPE
        {
            MOUSE = 0,
            KEYBOARD = 1,
            HARDWARE = 2,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public INPUT_TYPE type;

            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }

        [DllImport(User32, SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        #endregion
    }
}