using NetworkScanner.Application.Common.Interface;
using System;
using System.IO;
using System.IO.Pipes;

namespace NetworkScanner.Infrastructure.Display
{
    public class WriteToConsole : IDisplayResult
    {
        public NamedPipeServerStream PipeServer { get; set; }
        public StreamWriter PipeWriter { get; set; }

        public void Display(string value)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(value);
        }
    }
}
