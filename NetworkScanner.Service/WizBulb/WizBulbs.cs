using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkScanner.Service.Worker;
using NetworkScanner.Infrastructure.Factory;
using NetworkScanner.Domain.Entities;
using System.Linq;
using System.Collections.Generic;

namespace NetworkScanner.Service.WizBulb
{
    public class WizBulbs
    {
        protected ILogger Logger;
        private IBackgroundTaskQueue taskQueue;
        private List<Result> results;

        public WizBulbs(IBackgroundTaskQueue queue)
        {
            Logger = Log.ForContext<WizBulbs>();
            taskQueue = queue;
            results = new List<Result>();
        }

        public async Task WizBulbBuilder()
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(new TimeSpan(0, 0, 30));
                CancellationToken token = cts.Token;

                var WizBulbWorker = new WizBulbWorker(taskQueue, Logger);

                token.ThrowIfCancellationRequested();
                await WizBulbWorker.ExecuteAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WizBulbQueueItems()
        {
            var WizFactory = new WizBulbFactory();
            foreach (var item in FoundDeviceCollection.collection.Where(x => x.Value.DeviceName != Utils.Common).Select(x => x.Value).Reverse())
            {
                taskQueue.QueueBackgroundWorkItem(async _ =>
                {
                    var result = await WizFactory
                        .FindValue(item.IpAddress, "").ConfigureAwait(false);
                    results.Add(result);
                });
            }
        }
    }
}
