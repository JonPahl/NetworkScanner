//using MediatR.Pipeline;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NetworkScanner.Service.Pong
//{
//    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
//    {
//        private readonly TextWriter _writer;

//        public GenericRequestPreProcessor(TextWriter writer)
//        {
//            _writer = writer;
//        }

//        public Task Process(TRequest request, CancellationToken cancellationToken)
//        {
//            return _writer.WriteLineAsync("- Starting Up");
//        }
//    }
//}
