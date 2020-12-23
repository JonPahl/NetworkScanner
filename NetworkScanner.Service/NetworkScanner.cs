using Microsoft.Extensions.Hosting;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Display;
using NetworkScanner.Infrastructure.IpFinder;
using NetworkScanner.Service.WizBulb;
using NetworkScanner.Service.Worker;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkScanner.Service
{
    /// <summary>
    /// Scan network IP Addresses
    /// </summary>
    /// <seealso cref="IHostedService"/>
    /// <seealso cref="IDisposable"/>
    public class NetworkScanner : IHostedService, IDisposable
    {
        private bool isDisposed;
        public int QueueLength;
        private readonly ILogger Logger;
        private readonly object lockObj;
        private readonly Stopwatch stopWatch;
        private static System.Timers.Timer _timer;
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly SsdpWorker ssdpWorker;
        private readonly MdnsWorker mdnsWorker;
        private readonly IpAddressWorker IpWorker;
        private readonly DisplayResults Display;
        private readonly WizBulbs WizBulbBuilder;

        #region Setup

        public NetworkScanner(ILiteDbContext dbContext)
        {
            QueueLength = 0;

            Display = new DisplayResults(dbContext, new WriteToNamedPipe());
            Logger = Log.ForContext<NetworkScanner>();
            taskQueue = new BackgroundTaskQueue();
            lockObj = new object();
            ssdpWorker = new SsdpWorker();
            mdnsWorker = new MdnsWorker();
            IpWorker = new IpAddressWorker();
            stopWatch = new Stopwatch();
            WizBulbBuilder = new WizBulbs(taskQueue);

            FoundDeviceCollection.Changed += FoundDeviceCollection_Changed;
            SetupTimer(0, 5, 0);
        }

        #region Dispose Section

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                stopWatch.Stop();
                Display.Dispose();
            }

            isDisposed = true;
        }

        #endregion

        /// <summary>
        /// pass in code set IP addresses.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            #region TODO: do this better;

            var newAddressRange = new ScanAddresses
            {
                StartAddress = "192.168.1.1",
                EndAddress = "192.168.1.254",
                IsActive = true,
                Inserted = DateTime.Now
            };

            var updateIp = new UpdateIpAddresses();
            updateIp.InsertAddress(newAddressRange);

            #endregion

            Console.WriteLine($"Service Started: {DateTime.Now}");
            await RunNetworkScanner().ConfigureAwait(false);
            Console.WriteLine("Setup Finished");

            while (QueueLength != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"# {QueueLength} Items: {DateTime.Now} ");
                Thread.Sleep(1000);
            }

            Console.WriteLine();
            Console.WriteLine("Start Listeners");

            await WizBulbBuilder.WizBulbBuilder().ConfigureAwait(false);

            _timer.Enabled = true;
            _timer.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (QueueLength != 0)
                {
                    while (QueueLength != 0)
                    {
                        Thread.Sleep(1000);
                    }
                }

                Thread.Sleep(500);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        #endregion

        #region Timer
        private void SetupTimer(int hr = 0, int min = 5, int sec = 0)
        {
            var delay = new TimeSpan(hr, min, sec);
            var timer = new System.Timers.Timer(delay.TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = false;
            _timer = timer;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            Console.WriteLine($"NetworkScanner @{DateTime.Now}");

            IpWorker.RunAsync().GetAwaiter().GetResult();

            ssdpWorker.RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            mdnsWorker.Run();

            _timer.Enabled = true;
            _timer.Start();
        }

        #endregion

        private async Task RunNetworkScanner()
            => await IpWorker.RunAsync().ConfigureAwait(false);

        private void FoundDeviceCollection_Changed(object sender, FoundDeviceChangedEventArgs e)
        {
            lock (lockObj)
            {
                Display.BuildTable();
            }
        }
    }
}