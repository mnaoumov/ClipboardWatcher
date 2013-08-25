using System;

namespace JLorek.Samples.ClipboardObserver
{
    class Program
    {
        static void Main()
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("Press [RETURN] to quit...");

            var clipboardObserver = new ClipboardObserver();
            clipboardObserver.ClipboardTextChanged += text => Console.WriteLine(string.Format("Text arrived @ clipboard: {0}", text));

            Console.ReadLine();
        }
    }
}
