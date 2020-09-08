using NetworkScanner.Entities;
using NetworkScanner.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    /// <summary>
    /// Generic async ping class.
    /// </summary>
    public class PingHosts
    {
        private List<string> ipAddresses = new List<string>();

        private readonly int timeout;
        private int nFound;
        private readonly object lockObj;
        private readonly Stopwatch stopWatch;

        public PingHosts()
        {
            lockObj = new object();
            stopWatch = new Stopwatch();

            nFound = 0;
            timeout = 5000;
        }

        /// <summary>
        /// Runs the ping.
        /// </summary>
        public void RunPing()
        {
            nFound = 0;
            var tasks = new List<Task>();
            stopWatch.Start();

            foreach (var ip in ipAddresses)
            {
                var task = PingAndUpdateAsync(new Ping(), ip);
                tasks.Add(task);
            }
        }

        internal void SetIpAddresses(List<string> ipRanges)
        {
            (ipAddresses ??= new List<string>()).AddRange(ipRanges);
        }

        private async Task PingAndUpdateAsync(Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout).ConfigureAwait(false);

            if (reply.Status == IPStatus.Success)
            {
                var foundDevice = BuildDeviceObject.BuildDevice(ip, "Ping");

                FoundDeviceCollection.Add(foundDevice);

                lock (lockObj)
                {
                    nFound++;
                }
            }
        }

    }
}