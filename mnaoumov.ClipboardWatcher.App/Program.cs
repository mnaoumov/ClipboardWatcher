using System;

namespace mnaoumov.ClipboardWatcher.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press [RETURN] to quit...");

            using (var clipboardWatcher = new global::ClipboardWatcher())
            {
                clipboardWatcher.ClipboardTextChanged += text => Console.WriteLine(string.Format("Text arrived @ clipboard: {0}", text));
                Console.ReadLine();
            } 
        }
    }
}
