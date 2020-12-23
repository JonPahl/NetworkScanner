using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.HostName;
using NetworkScanner.Infrastructure.IpFinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkScanner.Service.Worker
{
    public class IpAddressWorker : BackgroundWorker
    {
        private FindIpAddresses Finder;

        private int nFound;
        private readonly Stopwatch stopWatch;

        private readonly HashSet<string> FoundIpAddresses;
        private int Timeout { get; }

        private PingWorkflow Workflow = null;
        public int QueueLength;
        private readonly object lockObj;
        public List<string> IpAddresses { get; }

        public IpAddressWorker()
        {
            Finder = new FindIpAddresses();
            stopWatch = new Stopwatch();

            IpAddresses = new List<string>();
            FoundIpAddresses = new HashSet<string>();

            Timeout = 2000;
            QueueLength = 0;
            lockObj = new object();
        }

        public async Task RunAsync()
        {
            var ranges = Finder.LoadAddresses;
            Finder.BuildIpRanges(ranges.ToList());
            IpAddresses.AddRange(Finder.IpAddresses);

            await ScanIpAddresses().ConfigureAwait(false);
        }

        private async Task ScanIpAddresses()
        {
            nFound = 0;

            var tasks = new List<Task>();
            stopWatch.Start();

            tasks.AddRange(from ip in IpAddresses
                           select PingAndUpdateAsync(new Ping(), ip));
            await Task
                .WhenAll(tasks)
                .ContinueWith(_ =>
                {
                    stopWatch.Stop();
                    var ts = stopWatch.Elapsed;

                    Console.WriteLine(
                        $"{nFound} devices found! Elapsed time: {ts}", "Asynchronous");
                }).ConfigureAwait(false);

            ProcessFoundAddresses();
        }

        private async Task PingAndUpdateAsync(Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, Timeout).ConfigureAwait(false);

            if (reply.Status != IPStatus.Success)
                return;

            lock (lockObj)
            {
                var returnedIp = reply.Address.ToString();
                FoundIpAddresses.Add(returnedIp);
                nFound++;
            }
        }

        private void ProcessFoundAddresses()
        {
            foreach (var ip in FoundIpAddresses)
            {
                try
                {
                    Workflow = new PingWorkflow();

                    var foundDevice = Workflow.BuildObject<FoundDevice>(ip);
                    var DeviceName = Workflow.FindDeviceName(foundDevice);
                    foundDevice.DeviceName = DeviceName.Value ?? Utils.Common;
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(FindDeviceId), foundDevice);
                    QueueLength++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void FindDeviceId(object obj)
        {
            try
            {
                var workflow = new PingWorkflow();

                var device = obj as FoundDevice;
                var DeviceId = workflow.FindDeviceId(device);
                device.DeviceId = DeviceId.Value ?? Utils.Common;
                FoundDeviceCollection.Add(device);

                Thread.Sleep(100);
                QueueLength--;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
