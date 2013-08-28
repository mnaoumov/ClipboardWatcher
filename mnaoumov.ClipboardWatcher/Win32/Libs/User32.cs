using System;
using System.Runtime.InteropServices;

namespace mnaoumov.ClipboardWatcher.Win32.Libs
{
    /// <summary>
    ///     P/Invoke functions from user32.dll.
    /// </summary>
    public class User32
    {
        const string User32Dll = "User32.dll";

        /// <summary>
        ///     Places the given window in the system-maintained clipboard format listener list.
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ms649033.aspx
        ///     Windows SDK - WinUser.h
        /// </summary>
        /// <param name="hWndObserver"></param>
        /// <returns></returns>
        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern bool AddClipboardFormatListener(IntPtr hWndObserver);

        /// <summary>
        ///     Removes the given window from the system-maintained clipboard format listener list.
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ms649050.aspx
        ///     Windows SDK - WinUser.h
        /// </summary>
        /// <param name="hWndObserver"></param>
        /// <returns></returns>
        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hWndObserver);
    }
}