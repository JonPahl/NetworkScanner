using Microsoft.Extensions.Hosting;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Application.Compare;
using NetworkScanner.Domain.Display;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure;
using NetworkScanner.Infrastructure.Database;
using NetworkScanner.Infrastructure.Display;
using NetworkScanner.Infrastructure.HostName;
using NetworkScanner.Service.Worker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        public int QueueLength;
        private List<string> IpAddresses;
        private readonly RangeFinder IpRange;
        private readonly HashSet<string> FoundIpAddresses;
        private readonly int timeout;
        private int nFound;
        private readonly object lockObj;
        private readonly Stopwatch stopWatch;
        private PingWorkflow Workflow = null;
        private static System.Timers.Timer _timer;

        private readonly IDisplayResult displayResult;
        private readonly SsdpWorker ssdpWorker;
        private readonly MdnsWorker mdnsWorker;
        private readonly object _object;

        #region Setup

        public NetworkScanner()
        {
            //var pipeName = "TestOne";

            QueueLength = 0;
            timeout = 2000;

            _object = new object();
            ssdpWorker = new SsdpWorker();
            mdnsWorker = new MdnsWorker();

            displayResult = new WriteToNamedPipe();
            lockObj = new object();
            stopWatch = new Stopwatch();

            IpRange = new RangeFinder();
            IpAddresses = new List<string>();
            FoundIpAddresses = new HashSet<string>();

            FoundDeviceCollection.Changed += FoundDeviceCollection_Changed;
            SetupTimer(0, 5, 0);
        }

        public void Dispose()
        {
            stopWatch.Stop();
            displayResult.PipeWriter.Close();
            displayResult.PipeServer.Close();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Service Started: {DateTime.Now}");
            IpAddresses = IpRange.GetIPRange(IPAddress.Parse("192.168.1.1"), IPAddress.Parse("192.168.1.254")).ToList();
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

            await ssdpWorker.RunAsync().ConfigureAwait(false);
            mdnsWorker.Run();

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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion

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
            ScanIpAddresses().GetAwaiter().GetResult();

            ssdpWorker.RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            mdnsWorker.Run();

            _timer.Enabled = true;
            _timer.Start();
        }

        private async Task RunNetworkScanner()
        {
            await ScanIpAddresses().ConfigureAwait(false);
        }
        private async Task ScanIpAddresses()
        {
            nFound = 0;

            var tasks = new List<Task>();
            stopWatch.Start();

            tasks.AddRange(from ip in IpAddresses select PingAndUpdateAsync(new Ping(), ip));

            await Task.WhenAll(tasks).ContinueWith(_ =>
            {
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine(nFound.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
            }).ConfigureAwait(false);

            ProcessesFoundAddresses();
        }

        public void FindDeviceId(object obj)
        {
            try
            {
                var fd = obj as FoundDevice;
                var workflow = new PingWorkflow();
                var DeviceId = workflow.FindDeviceId(fd);
                fd.DeviceId = DeviceId.Value ?? Utils.Common;
                FoundDeviceCollection.Add(fd);

                Thread.Sleep(100);
                QueueLength--;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FoundDeviceCollection_Changed(object sender, FoundDeviceChangedEventArgs e)
        {
            lock (_object)
            {
                BuildTable();
            }
        }

        #region Process found IP Addresses
        private void ProcessesFoundAddresses()
        {
            foreach (var ip in FoundIpAddresses)
            {
                try
                {
                    Workflow = new PingWorkflow();

                    var fd = Workflow.BuildObject<FoundDevice>(ip);
                    var DeviceName = Workflow.FindDeviceName(fd);
                    fd.DeviceName = DeviceName.Value ?? Utils.Common;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FindDeviceId), fd);
                    QueueLength++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        #endregion

        #region Ping Asynchronous
        private async Task PingAndUpdateAsync(Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout).ConfigureAwait(false);

            if (reply.Status != IPStatus.Success)
                return;

            lock (lockObj)
            {
                var returnedIp = reply.Address.ToString();
                FoundIpAddresses.Add(returnedIp);
                nFound++;
            }
        }

        #endregion

        #region Display Results

        private void BuildTable()
        {
            try
            {
                var devices = FoundDeviceCollection.collection.Select(x => x.Value).OrderBy(x => x, new FoundDeviceCompare());
                var cnt = 1;
                foreach (var device in devices)
                {
                    device.Id = cnt.ToString();
                    device.Key = device.GetHashCode();
                    cnt++;
                }

                #region Store to Db
                var ctx = new NetworkContext();

                foreach (var device in devices)
                {
                    var found = ctx.KeyExists(device);
                    if (found)
                    {
                        ctx.Update(device);
                    }
                    else
                    {
                        ctx.Insert(device);
                    }
                }
                #endregion

                var tempWrite = DisplayTable(devices.Select(x => x).ToList());
                displayResult.Display(tempWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string DisplayTable(List<FoundDevice> devices)
        {
            return devices.ToStringTable(
                new[] { "ID", "KEY", "IP ADDRESS", "DEVICE NAME", "DEVICE ID", "FOUND USING", "TIMESTAMP" },
                a => a.Id, a => a.Key, a => a.IpAddress, a => a.DeviceName, a => a.DeviceId, a => a.FoundUsing, a => a.FoundAt);
        }

        #endregion Display Results
    }
}