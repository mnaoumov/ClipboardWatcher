using System;

namespace ClipboardObserver
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Press [RETURN] to quit...");

            var clipboardObserver = new ClipboardObserver();
            clipboardObserver.ClipboardTextChanged += text => Console.WriteLine(string.Format("Text arrived @ clipboard: {0}", text));

            Console.ReadLine();
        }
    }
}
