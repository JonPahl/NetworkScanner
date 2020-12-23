using System;
using System.IO;
using System.IO.Pipes;

namespace Output
{
    public static class Program
    {
        internal const string pipeName = "TestOne";
        public static void Main()
        {
            using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In);
            pipeClient.Connect();
            
            using StreamReader sr = new StreamReader(pipeClient);
            // Display the read text to the console
            string temp;

            while (!sr.EndOfStream)
            {
                temp = sr.ReadLine();
                Console.WriteLine(temp);

                if (sr.Peek().Equals(-1))
                    Console.SetCursorPosition(0, 0);
            }

            var x = 0;
            /*while ((temp = sr.ReadLine()) != null)
            {
                var line = sr.ReadLine();
                Console.WriteLine(sr.ReadLine());
                if (sr.Peek().Equals(-1))
                    Console.SetCursorPosition(0, 0);
            }*/
        }
    }
}