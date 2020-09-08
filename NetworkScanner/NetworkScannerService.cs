using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetworkScanner.Database;
using NetworkScanner.Entities;
using NetworkScanner.Network;
using NetworkScanner.Upnp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Waher.Networking.UPnP;

namespace NetworkScanner
{
    public class NetworkScannerService : BackgroundService
    {
        private readonly ILogger<NetworkScannerService> _logger;
        private List<string> IpRanges = new List<string>();

        //private readonly BonjourFinder bonjourFinder;

        private readonly UpnpSearcher UpnpSearcher;
        private readonly RssdpFinder rsdpFinder;
        private readonly SsdpFinder ssdp;
        private readonly PingHosts pingHosts;

        private System.Timers.Timer IpPingTimer;

        public NetworkScannerService(ILogger<NetworkScannerService> logger)
        {
            _logger = logger;

            FoundDeviceCollection.Changed += CollectionChanged;
            IpRanges.AddRange(BuildIpRange("192.168.1.1", "192.168.1.254"));

            #region Setup

            //bonjourFinder = new BonjourFinder();

            UpnpSearcher = new UpnpSearcher();
            rsdpFinder = new RssdpFinder();
            ssdp = new SsdpFinder();

            pingHosts = new PingHosts();
            pingHosts.SetIpAddresses(IpRanges);
            SetupTimer();

            #endregion
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var mDns = new MdnsLookup();

            //FindReachableHosts();
            
            #region Start Listening
            //bonjourFinder.StartStopListener();

            //rsdpFinder.StartListening();
            ssdp.StartListening();

            await UpnpSearcher.BeginSearch().ConfigureAwait(false);
            #endregion Start Listening

            IpPingTimer.Enabled = true;
            IpPingTimer.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                Thread.Sleep(250);
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
            }
        }

        private void FindReachableHosts()
        {
            pingHosts.RunPing();
        }

        #region Timer

        private void SetupTimer()
        {
            var delay = new TimeSpan(00, 05, 00).TotalMilliseconds;
            IpPingTimer = new System.Timers.Timer(delay);
            IpPingTimer.Elapsed += IpPing_Elapsed;
        }

        private void IpPing_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IpPingTimer.Stop();
            FindReachableHosts();
            IpPingTimer.Start();
        }

        #endregion


        #region Process table

        private void CollectionChanged(object sender, FoundDeviceChangedEventArgs e)
        {
            BuildTable();

            //var db = new ElasticSearchContext();
            //var x = db.Insert<FoundDevice>(e.ChangedItem);

            var db = new NetworkContext();

            if (db.KeyExists<FoundDevice>(e.ChangedItem))
            {
                //run update.
            } else
            {
                var results = db.Insert<FoundDevice>(e.ChangedItem);
                Console.WriteLine(results);
            }

            /*
            var m = new MacVenderFinder();
            var mac = m.GetMacByIP(e.ChangedItem.IpAddress).GetAwaiter().GetResult();
            System.Net.NetworkInformation.PhysicalAddress r = new System.Net.NetworkInformation.PhysicalAddress(Encoding.ASCII.GetBytes(mac));

            var macBuilder = "NA";
            if (mac != "N/A")
                macBuilder = LookupMac(mac).GetAwaiter().GetResult();
            */
            //Console.WriteLine($"{e.ChangedItem.IpAddress}:\t{mac}\t\t{macBuilder}");
        }


        private void BuildTable()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            var devices = new List<Tuple<int, string, string, string, string, string>>();

            var i = 1;
            foreach (var item in FoundDeviceCollection.collection.OrderBy(x => x.Value, new IPComparer()))
            {
                var deviceID = item.Value.DeviceId ?? "N/A";
                var ipAddress = item.Value.IpAddress;
                var name = item.Value.DeviceName ?? "N/A";
                var foundUsing = item.Value.FoundUsing;
                var foundAt = item.Value.FoundAt != null ? item.Value.FoundAt.ToString() : DateTime.Now.ToString();

                var tuple = Tuple.Create(i, ipAddress, name, deviceID, foundUsing, foundAt);
                devices.Add(tuple);
                i++;
            }

            Console.WriteLine(DisplayTable(devices));
        }

        private string DisplayTable(List<Tuple<int, string, string, string, string, string>> devices)
        {
            return devices.ToStringTable(
                              new[] { "Id", "Ip Address", "Device Name", "Device Id", "Found Using", "TimeStamp" },
                              a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6);
        }

        #endregion


        #region Build IpRange
        private IList<string> BuildIpRange(string start, string end)
        {
            var startAddress = IPAddress.Parse(start);
            var endAddress = IPAddress.Parse(end);
            var ipRange = new RangeFinder();
            var range = ipRange.GetIPRange(startAddress, endAddress);

            return range.Any() ? range.ToList() : new List<string>();
        }

        #endregion

    }
}