using System;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardObserver
{
    internal class ClipboardObserver
    {
        private readonly Thread _formThread;
        private ClipboardObserverForm _observerForm;

        public ClipboardObserver()
        {
            _formThread = new Thread(() =>
                                     {
                                         _observerForm = new ClipboardObserverForm();
                                         _observerForm.ClipboardTextChanged += text => ClipboardTextChanged(text);
                                         Application.Run(_observerForm);
                                     })
                          {
                              IsBackground = true
                          };

            _formThread.SetApartmentState(ApartmentState.STA);
            _formThread.Start();
        }

        ~ClipboardObserver()
        {
            if (_observerForm != null)
                _observerForm.Close();

            if (_formThread != null && _formThread.IsAlive)
                _formThread.Abort();
        }

        public event Action<string> ClipboardTextChanged = delegate { };
    }
}