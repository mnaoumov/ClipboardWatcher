using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardObserver
{
    public partial class ClipboardObserverForm : Form
    {
        private IntPtr _nextClipboardViewer;

        internal ClipboardObserverForm(ClipboardObserver clipboardObserver)
        {
            InitializeComponent();
            HideForm();
            RegisterClipboardViewer();
            ClipboardTextChanged += clipboardObserver.OnClipboardTextChanged;
            clipboardObserver.Disposed += Dispose;
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
            if (IsClipboardFormatListenerAvailiable())
                User32.AddClipboardFormatListener(Handle);
            else
                _nextClipboardViewer = User32.SetClipboardViewer(Handle);
        }

        private void UnregisterClipboardViewer()
        {
            if (IsClipboardFormatListenerAvailiable())
                User32.RemoveClipboardFormatListener(Handle);
            else
                User32.ChangeClipboardChain(Handle, _nextClipboardViewer);
        }

        /// <summary>
        ///     http://stackoverflow.com/questions/2819934/detect-windows-7-in-net
        /// </summary>
        private bool IsClipboardFormatListenerAvailiable()
        {
            return Environment.OSVersion.Version.Major >= 6;
        }

        protected override void WndProc(ref Message m)
        {
            switch ((User32.Message) m.Msg)
            {
                case User32.Message.WM_DRAWCLIPBOARD:
                    ClipboardChanged();

                    User32.SendMessage(
                        _nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case User32.Message.WM_CHANGECBCHAIN:
                    if (m.WParam == _nextClipboardViewer)
                        _nextClipboardViewer = m.LParam;
                    else
                        User32.SendMessage(_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case User32.Message.WM_CLIPBOARDUPDATE:
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