using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkScanner.Infrastructure.PipeLine
{
    public interface IPipelineBuilderStep<TIn, TOut>
    {
        IPipelineBuilderStep<TIn, TNewStepOut> AddStep<TNewStepOut>(Func<TOut, TNewStepOut> stepFunc, int workerCount);
        IPipeline<TIn, TOut> Create();
    }
}
