using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Machine.Specifications;

namespace ClipboardObserver
{
    internal class when_i_use_the_ClipboardObserver
    {
        private Establish context = () =>
        {
            sut = new ClipboardObserver();
            sut.ClipboardTextChanged += text =>
            {
                result = text;
            };
        };

        private Because of = () =>
        {
            while (string.IsNullOrEmpty(result))
            {
                Clipboard.SetText("foobar!");
                Debug.WriteLine("Waiting for some text to arrive in the clipboard.");
                Thread.Sleep(500);
            }
        };

        private It should_work = () =>
        {
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
        };

        internal static ClipboardObserver sut;
        internal static string result;
    }
}
