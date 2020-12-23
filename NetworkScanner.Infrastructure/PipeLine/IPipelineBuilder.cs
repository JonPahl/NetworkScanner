using System;

namespace NetworkScanner.Infrastructure.PipeLine
{
    /// <summary>
    /// Pipe line Builder
    /// </summary>
    public interface IPipelineBuilder
    {
        IPipelineBuilderStep<TStepIn, TStepOut> Build<TStepIn, TStepOut>(Func<TStepIn, TStepOut> stepFunc, int workerCount);
    }
}
