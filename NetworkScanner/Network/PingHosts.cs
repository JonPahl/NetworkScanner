using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    public class PingHosts
    {
        private SsdpFinder ssdp = new SsdpFinder();
        private List<string> ipAddresses = new List<string>();

        private int timeout = 100;
        private int nFound = 0;

        private static object lockObj = new object();
        private Stopwatch stopWatch = new Stopwatch();
        private TimeSpan ts;

        public async Task RunPingAsync()
        {
            nFound = 0;
            var tasks = new List<Task>();
            stopWatch.Start();

            foreach (var ip in ipAddresses)
            {
                Ping p = new Ping();
                var task = PingAndUpdateAsync(p, ip);
                tasks.Add(task);                
            }

            await Task.WhenAll(tasks)
                .ContinueWith(_ =>
                {
                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    Console.WriteLine(nFound.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
                }).ConfigureAwait(false);
        }

        internal void SetIpAddresses(List<string> ipRanges)
        {
            if (ipAddresses == null)
                ipAddresses = new List<string>();

            ipAddresses.AddRange(ipRanges);
        }

        private async Task PingAndUpdateAsync(Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout).ConfigureAwait(false);

            if (reply.Status == IPStatus.Success)
            {
                await ssdp.LoadData(ip).ConfigureAwait(false);

                lock (lockObj)
                {
                    nFound++;
                }
            }
        }
    }
}
