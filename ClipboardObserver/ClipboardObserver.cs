using System;
using System.Threading;

namespace ClipboardObserver
{
    internal class ClipboardObserver : IDisposable
    {
        private readonly Thread _formThread;
        private ClipboardObserverForm _observerForm;
        private bool _disposed;

        public ClipboardObserver()
        {
            _formThread = new Thread(() =>
                                     {
                                         _observerForm = new ClipboardObserverForm(this);
                                     })
                          {
                              IsBackground = true
                          };

            _formThread.SetApartmentState(ApartmentState.STA);
            _formThread.Start();
        }

        ~ClipboardObserver()
        {
            Dispose();
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

        public event Action<string> ClipboardTextChanged = delegate { };
        public event Action Disposed = delegate { }; 

        public void OnClipboardTextChanged(string text)
        {
            ClipboardTextChanged(text);
        }
    }
}