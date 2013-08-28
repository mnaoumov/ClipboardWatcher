using System;
using System.Drawing;
using System.Windows.Forms;
using mnaoumov.ClipboardWatcher.Win32.Enums;
using mnaoumov.ClipboardWatcher.Win32.Libs;

namespace mnaoumov.ClipboardWatcher
{
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
}