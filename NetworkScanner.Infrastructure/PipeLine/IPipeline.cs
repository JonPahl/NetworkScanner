using System;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.PipeLine
{
    /// <summary>
    /// Pipeline pipe builder
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IPipeline<TIn, TOut> : IDisposable
    {
        // TODO: use ValueTask + IValueTaskSource to avoid allocations
        Task<TOut> Execute(TIn data);
    }
}
