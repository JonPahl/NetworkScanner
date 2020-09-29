using System;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.PipeLine
{
    public interface IPipeline<TIn, TOut> : IDisposable
    {
        // TODO: use ValueTask + IValueTaskSource to avoid allocations
        Task<TOut> Execute(TIn data);
    }
}
