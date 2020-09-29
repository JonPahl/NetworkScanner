using NetworkScanner.Application.EventArgs;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkScanner.Application
{
    /// <summary>
    /// Pings provided IP addresses and checks which return a value.
    /// </summary>
    public class PingHosts
    {
        private List<string> ipAddresses = new List<string>();
        private readonly int timeout = 100;
        //private int nFound = 0;
        //private object lockObj;
        private readonly Stopwatch stopWatch;

        /// <summary>
        /// Occurs when [event completed].
        /// </summary>
        public event EventHandler<DeviceFoundEventArgs> EventHostFound;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingHosts"/> class.
        /// </summary>
        public PingHosts()
        {
            //lockObj = new object();
            stopWatch = new Stopwatch();
            //PingReply = new List<PingReply>();
        }

        protected void OnDeviceReached(DeviceFoundEventArgs e) => EventHostFound?.Invoke(this, e);

        /// <summary>
        /// Sets the ip addresses.
        /// </summary>
        /// <param name="ipRanges">The ip ranges.</param>
        public void SetIpAddresses(IList<string> ipRanges)
        {
            (ipAddresses ??= new List<string>()).AddRange(ipRanges);
        }

        /// <summary>
        /// Runs the ping.
        /// </summary>
        public void RunPing()
        {
            //nFound = 0;
            List<Task<PingReply>> tasks = new List<Task<PingReply>>();

            stopWatch.Start();

            foreach (var ip in ipAddresses)
            {
                tasks.Add(PingAndUpdateAsync(new Ping(), ip));
            }

            Task.WaitAll(tasks.ToArray());

            var reachable = tasks.Where(x => x.Result?.Status == IPStatus.Success).ToList();
            var cnt = 1;
            foreach (var result in reachable)
            {
                var reply = result.Result;

                var fd = new FoundDevice
                {
                    IpAddress = reply.Address.ToString(),
                    DeviceId = Utils.Common,
                    DeviceName = Utils.Common,
                    Id = cnt.ToString(),
                    FoundAt = DateTime.Now,
                    FoundUsing = "Ping"
                };

                var args = new DeviceFoundEventArgs
                {
                    Found = cnt,
                    TimeReached = DateTime.Now,
                    Item = fd
                };
                OnDeviceReached(args);
                cnt++;
            }
        }

        private async Task<PingReply> PingAndUpdateAsync(Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout).ConfigureAwait(false);
            return (reply.Status.Equals(IPStatus.Success)) ? reply : null;
        }
    }
}