using MediatR.Pipeline;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace NetworkScanner.Service.Pong.ExceptionHandler
{
    public class LogExceptionAction : RequestExceptionAction<Ping>
    {
        private readonly TextWriter _writer;

        public LogExceptionAction(TextWriter writer) => _writer = writer;

        protected override void Execute(Ping request, Exception exception)
        {
            _writer.WriteLineAsync($"--- Exception: '{exception.GetType().FullName}'");
        }
    }
}
