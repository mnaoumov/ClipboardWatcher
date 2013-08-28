namespace mnaoumov.ClipboardWatcher.Win32.Enums
{
    /// <summary>
    /// Clipboard Notifications
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ff468804.aspx
    /// Windows SDK - WinUser.h.
    /// </summary>
    public enum WM
    {
        /// <summary>
        /// Sent when the contents of the clipboard have changed.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms649021.aspx
        /// </summary>
        WM_CLIPBOARDUPDATE = 0x031D
    }
}