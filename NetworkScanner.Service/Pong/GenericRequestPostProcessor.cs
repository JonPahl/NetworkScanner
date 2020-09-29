//using MediatR.Pipeline;
//using NetworkScanner.Service.Entities;
//using System;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NetworkScanner.Service.Pong
//{
//    public class GenericRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
//    {
//        private readonly TextWriter _writer;

//        public GenericRequestPostProcessor(TextWriter writer)
//        {
//            _writer = writer;
//        }

//        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
//        {
//            //try
//            //{
//                var device = response as FoundDevice;

//                if (device.IpAddress != null)
//                {
//                    return _writer.WriteLineAsync($"{device.IpAddress}: \t\t {device.DeviceName} | {device.Cnt}");
//                } else
//                {
//                    throw new Exception("No Response");
//                }
//            //}
//            //catch (Exception ex)
//            //{
//            //    throw;
//            //}
//            //throw new Exception("No Response");
//        }
//    }
//}
