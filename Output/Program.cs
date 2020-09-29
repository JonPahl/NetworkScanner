using System;
using System.IO;
using System.IO.Pipes;

namespace Output
{
    public static class Program
    {
        internal const string pipeName = "TestOne";

        //[STAThread]        
        public static void Main()
        {
            //Console.SetWindowPosition(0, 0);
            //Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            NamedPipeClientStream PipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In);
            PipeClient.Connect();
            StreamReader PipeReader = new StreamReader(PipeClient);
            while (!PipeReader.EndOfStream)
            {
                var result = PipeReader.ReadLine();
                //if(result.Equals("9999"))
                //{
                //    Thread.Sleep(new TimeSpan(0, 0, 15));
                //Console.Clear();
                //Console.SetCursorPosition(0, 0);
                //}
                //else
                //{
                Console.WriteLine(result);
                if (PipeReader.Peek().Equals(-1))
                {
                    //Console.Clear();
                    Console.SetCursorPosition(0, 0);
                }
                //}
            }

            PipeReader.Close();
            PipeClient.Close();
        }
    }
}