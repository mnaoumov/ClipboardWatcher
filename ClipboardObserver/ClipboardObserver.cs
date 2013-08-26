using System;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardObserver
{
    class ClipboardObserver
    {
        private ClipboardObserverForm _observerForm;
        private readonly Thread _formThread;

        public ClipboardObserver()
        {
            _formThread = new Thread(() =>
            {
                _observerForm = new ClipboardObserverForm();
                _observerForm.ClipboardTextChanged += text =>
                {
                    ClipboardTextChanged(text);
                };
                Application.Run(_observerForm);
            });

            _formThread.IsBackground = true;
            _formThread.SetApartmentState(ApartmentState.STA);
            _formThread.Start();
        }

        ~ClipboardObserver()
        {
            if (_observerForm != null)
            {
                _observerForm.Close();
            }

            if ((_formThread != null) && (_formThread.IsAlive))
            {
                _formThread.Abort();
            }
        }

        public event Action<string> ClipboardTextChanged = delegate { };
    }
}
