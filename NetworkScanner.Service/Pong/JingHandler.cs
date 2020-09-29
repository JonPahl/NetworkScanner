//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NetworkScanner.Service.Pong
//{
//    public class JingHandler : AsyncRequestHandler<Jing>
//    {
//        private readonly TextWriter _writer;

//        public JingHandler(TextWriter writer)
//        {
//            _writer = writer;
//        }

//        protected override Task Handle(Jing request, CancellationToken cancellationToken)
//        {
//            return _writer.WriteLineAsync($"--- Handled Jing: {request.Message}, no Jong");
//        }
//    }
//}
