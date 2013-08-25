using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardNotifier
{
    public partial class ClipboardObserverForm : Form
    {
        private IntPtr _nextClipboardViewer;

        public event Action<string> ClipboardTextChanged = delegate { };

        internal ClipboardObserverForm()
        {
            InitializeComponent();
            HideForm();
            RegisterClipboardViewer();
        }

        private void HideForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Load += (sender, args) => { this.Size = new Size(0, 0); };
        }

        private void RegisterClipboardViewer()
        {
            if (IsClipboardFormatListenerAvailiable())
            {
                User32.AddClipboardFormatListener(this.Handle);
            }
            else
            {
                _nextClipboardViewer = User32.SetClipboardViewer(this.Handle);
            }

            this.Closed += (sender, args) => { UnregisterClipboardViewer(); };
        }

        private void UnregisterClipboardViewer()
        {
            if (IsClipboardFormatListenerAvailiable())
            {
                User32.RemoveClipboardFormatListener(this.Handle);
            }
            else
            {
                User32.ChangeClipboardChain(this.Handle, _nextClipboardViewer);
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/2819934/detect-windows-7-in-net
        /// </summary>
        private bool IsClipboardFormatListenerAvailiable()
        {
            return (Environment.OSVersion.Version.Major >= 6);
        }

        protected override void WndProc(ref Message m)
        {
            switch ((User32.Message) m.Msg)
            {
                case User32.Message.WM_DRAWCLIPBOARD:
                {
                    ClipboardChanged();

                    User32.SendMessage(
                        _nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                }
                break;

                case User32.Message.WM_CHANGECBCHAIN:
                {
                    if (m.WParam == _nextClipboardViewer)
                    {
                        _nextClipboardViewer = m.LParam;
                    }
                    else
                    {
                        User32.SendMessage(
                            _nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    }
                }
                break;

                case User32.Message.WM_CLIPBOARDUPDATE:
                {
                    ClipboardChanged();
                }
                break;

                default:
                {
                    base.WndProc(ref m);
                }
                break;
            }
        }

        private void ClipboardChanged()
        {
            if (Clipboard.ContainsText())
            {
                ClipboardTextChanged(Clipboard.GetText());
            }
        }
    }
}
