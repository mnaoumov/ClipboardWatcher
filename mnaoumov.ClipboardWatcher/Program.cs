using System;

namespace mnaoumov.ClipboardWatcher
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Press [RETURN] to quit...");

            using (var clipboardWatcher = new ClipboardWatcher())
            {
                clipboardWatcher.ClipboardTextChanged += text => Console.WriteLine(string.Format("Text arrived @ clipboard: {0}", text));
                Console.ReadLine();
            }

        }
    }
}
