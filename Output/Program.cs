using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace Output
{
    public static class Program
    {
        internal const string pipeName = "TestOne";

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static readonly IntPtr thisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;

        public static void Main()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(thisConsole, MAXIMIZE);

            using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In);
            pipeClient.Connect();
            using StreamReader sr = new StreamReader(pipeClient);

            while (!sr.EndOfStream)
            {
                var temp = sr.ReadLine();
                Console.WriteLine(temp);

                if (sr.Peek().Equals(-1))
                    Console.SetCursorPosition(0, 0);
            }
        }
    }
}