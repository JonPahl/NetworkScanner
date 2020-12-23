using Disruptor.Dsl;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.PipeLine.Build
{
    /// <summary>
    /// Disruptor Pipeline
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class DisruptorPipeline<TIn, TOut> : IPipeline<TIn, TOut>
    {
        private readonly Disruptor<DisruptorEvent> _disruptor;

        public DisruptorPipeline(Disruptor<DisruptorEvent> disruptor)
        {
            _disruptor = disruptor;
            _disruptor.Start();
        }

        public Task<TOut> Execute(TIn data)
        {
            // RunContinuationsAsynchronously to prevent continuation from "stealing" 
            // the releaser thread
            var tcs = new TaskCompletionSource<TOut>(TaskCreationOptions.RunContinuationsAsynchronously);

            var sequence = _disruptor.RingBuffer.Next();
            var disruptorEvent = _disruptor[sequence];
            disruptorEvent.Write(data);
            disruptorEvent.TaskCompletionSource = tcs;
            _disruptor.RingBuffer.Publish(sequence);

            return tcs.Task;
        }

        public void Dispose() => _disruptor.Shutdown();
    }
}