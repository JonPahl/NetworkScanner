//using MediatR.Pipeline;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

namespace NetworkScanner.Service.Pong.ExceptionHandler
{
    //public partial class CommonExceptionHandler : AsyncRequestExceptionHandler<PingResourceTimeout, Pong>
    //{

    //     private readonly TextWriter _writer;

    //     public CommonExceptionHandler(TextWriter writer)
    //     {
    //         this._writer = writer;
    //     }

    //     //        protected async override Task Handle(PingResourceTimeout request,
    //     //            Exception exception,
    //     //            RequestExceptionHandlerState<Pong> state,
    //     //            CancellationToken cancellationToken)
    //     //        {
    //     //            await _writer.WriteLineAsync($"---- Exception Handler: '{typeof(CommonExceptionHandler).FullName}'").ConfigureAwait(false);
    //     //            state.SetHandled(new Pong());
    //     //        }
    //     //        */
    // }

    //    public partial class ServerExceptionHandler : ServerExceptionHandler
    //    {
    //        /*
    //        private readonly TextWriter _writer;

    //        public ServerExceptionHandler(TextWriter writer) : base(writer) => _writer = writer;

    //        public async override Task Handle(PingNewResource request,
    //            ServerException exception,
    //            RequestExceptionHandlerState<Pong> state,
    //            CancellationToken cancellationToken)
    //        {
    //            await _writer.WriteLineAsync($"---- Exception Handler: '{typeof(ServerExceptionHandler).FullName}'").ConfigureAwait(false);
    //            state.SetHandled(new Pong());
    //        }
    //        */
    //    }
}
