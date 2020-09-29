//using MediatR.Pipeline;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NetworkScanner.Service.Pong
//{
//    public class ConstrainedRequestPostProcessor<TRequest, TResponse>
//        : IRequestPostProcessor<TRequest, TResponse>
//        where TRequest : Ping
//    {
//        private readonly TextWriter _writer;

//        public ConstrainedRequestPostProcessor(TextWriter writer)
//        {
//            _writer = writer;
//        }

//        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
//        {
//            return _writer.WriteLineAsync("- All Done with Ping");
//        }
//    }
//}