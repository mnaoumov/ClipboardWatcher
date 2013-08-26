using System;
using System.Runtime.InteropServices;

namespace ClipboardObserver.Win32.Libs
{
    /// <summary>
    /// P/Invoke functions from user32.dll.
    /// </summary>
    public class User32
    {
        const string User32Dll = "User32.dll";

        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        /// <param name="hWndObserver"></param>
        /// <returns></returns>
        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern bool AddClipboardFormatListener(IntPtr hWndObserver);

        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hWndObserver);
    }
}
