using NetworkScanner.Application.Common.Interface;
using System;
using System.IO;
using System.IO.Pipes;

namespace NetworkScanner.Infrastructure.Display
{
    public class WriteToNamedPipe : IDisplayResult
    {
        public NamedPipeServerStream PipeServer { get; set; }
        public StreamWriter PipeWriter { get; set; }

        public WriteToNamedPipe(string pipeName = "TestOne")
        {
            //var pipeName = "TestOne";
            PipeServer = new NamedPipeServerStream(pipeName, PipeDirection.Out);
            PipeServer.WaitForConnection();
            PipeWriter = new StreamWriter(PipeServer) { AutoFlush = true };
        }

        public void Display(string value)
        {
            try
            {
                PipeWriter.NewLine = "\r\n";
                PipeWriter.WriteLine(value);
                PipeWriter.Flush();
            }
            catch (IOException ex)
            {
                if (ex.Message == "Pipe is broken.")
                {
                    Console.Error.WriteLine("[NamedPipeServer]: NamedPipeClient was closed, exiting");
                    return;
                }
            }
        }
    }
}
