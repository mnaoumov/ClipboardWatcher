using System;
using System.Threading;

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
}