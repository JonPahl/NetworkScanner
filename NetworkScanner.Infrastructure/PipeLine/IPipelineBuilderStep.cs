using System;

namespace NetworkScanner.Infrastructure.PipeLine
{
    /// <summary>
    /// Build Pipeline
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IPipelineBuilderStep<TIn, TOut>
    {
        IPipelineBuilderStep<TIn, TNewStepOut> AddStep<TNewStepOut>(Func<TOut, TNewStepOut> stepFunc, int workerCount);
        IPipeline<TIn, TOut> Create();
    }
}
