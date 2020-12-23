using System.IO;
using System.IO.Pipes;

namespace NetworkScanner.Application.Common.Interface
{
    public interface IDisplayResult
    {
        public NamedPipeServerStream PipeServer { get; set; }
        public StreamWriter PipeWriter { get; set; }
        public void Display(string value);
    }
}
