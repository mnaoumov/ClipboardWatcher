using System;
using System.Drawing;
using System.Windows.Forms;
using ClipboardObserver.Win32.Enums;
using ClipboardObserver.Win32.Libs;
using Message = System.Windows.Forms.Message;

namespace ClipboardObserver
{
    public partial class ClipboardObserverForm : Form
    {
        internal ClipboardObserverForm(ClipboardObserver clipboardObserver)
        {
            InitializeComponent();
            HideForm();
            RegisterClipboardViewer();
            ClipboardTextChanged += clipboardObserver.OnClipboardTextChanged;
            clipboardObserver.Disposed += () => Invoke(new Action(Dispose));
            Disposed += (sender, args) => UnregisterClipboardViewer();
            Application.Run(this);
        }

        public event Action<string> ClipboardTextChanged = delegate { };

        private void HideForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Load += (sender, args) => { Size = new Size(0, 0); };
        }

        private void RegisterClipboardViewer()
        {
            User32.AddClipboardFormatListener(Handle);
        }

        private void UnregisterClipboardViewer()
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

        private void ClipboardChanged()
        {
            if (Clipboard.ContainsText())
                ClipboardTextChanged(Clipboard.GetText());
        }
    }
}