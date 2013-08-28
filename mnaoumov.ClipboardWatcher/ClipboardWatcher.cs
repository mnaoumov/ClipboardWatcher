using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace mnaoumov.ClipboardWatcher
{
    public class ClipboardWatcher : IDisposable
    {
        readonly Thread _formThread;
        bool _disposed;

        public ClipboardWatcher()
        {
            _formThread = new Thread(() => { new ClipboardWatcherForm(this); })
                          {
                              IsBackground = true
                          };

            _formThread.SetApartmentState(ApartmentState.STA);
            _formThread.Start();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            Disposed();
            if (_formThread != null && _formThread.IsAlive)
                _formThread.Abort();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        ~ClipboardWatcher()
        {
            Dispose();
        }

        public event Action<string> ClipboardTextChanged = delegate { };
        public event Action Disposed = delegate { };

        public void OnClipboardTextChanged(string text)
        {
            ClipboardTextChanged(text);
        }
    }

    public class ClipboardWatcherForm : Form
    {
        public ClipboardWatcherForm(ClipboardWatcher clipboardWatcher)
        {
            HideForm();
            RegisterWin32();
            ClipboardTextChanged += clipboardWatcher.OnClipboardTextChanged;
            clipboardWatcher.Disposed += () => Invoke(new Action(Dispose));
            Disposed += (sender, args) => UnregisterWin32();
            Application.Run(this);
        }

        public event Action<string> ClipboardTextChanged = delegate { };

        void HideForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Load += (sender, args) => { Size = new Size(0, 0); };
        }

        void RegisterWin32()
        {
            User32.AddClipboardFormatListener(Handle);
        }

        void UnregisterWin32()
        {
            User32.RemoveClipboardFormatListener(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            switch ((WM) m.Msg)
            {
                case WM.WM_CLIPBOARDUPDATE:
                    ClipboardChanged();
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void ClipboardChanged()
        {
            if (Clipboard.ContainsText())
                ClipboardTextChanged(Clipboard.GetText());
        }
    }

    /// <summary>
    ///     Clipboard Notifications
    ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ff468804.aspx
    ///     Windows SDK - WinUser.h.
    /// </summary>
    public enum WM
    {
        /// <summary>
        ///     Sent when the contents of the clipboard have changed.
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ms649021.aspx
        /// </summary>
        WM_CLIPBOARDUPDATE = 0x031D
    }

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