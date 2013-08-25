using System;
using System.Runtime.InteropServices;

namespace JLorek.Samples.ClipboardObserver
{
    class User32
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool AddClipboardFormatListener(IntPtr hWndObserver);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hWndObserver);

        public enum Message
        {
            WM_DRAWCLIPBOARD = 0x308,
            WM_CHANGECBCHAIN = 0x030D,
            WM_CLIPBOARDUPDATE = 0x031D
        }
    }
}
