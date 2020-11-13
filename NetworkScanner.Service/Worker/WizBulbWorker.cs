using Serilog;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkScanner.Service.Worker
{
    public class WizBulbWorker : BackgroundWorker
    {
        private readonly ILogger _logger;
        public IBackgroundTaskQueue TaskQueue { get; }

        public WizBulbWorker(IBackgroundTaskQueue taskQueue, ILogger logger)
        {
            TaskQueue = taskQueue;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information($"Queued Hosted Service is running.{Environment.NewLine}");
            await BackgroundProcessing(stoppingToken).ConfigureAwait(false);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken).ConfigureAwait(false);

                try
                {
                    await workItem(stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        /*
        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Queued Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
        */

    }

    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>>
            _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }
    }
}
