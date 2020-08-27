using NetworkScanner.Entities;
using NetworkScanner.Network;
using NetworkScanner.Upnp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetworkScanner
{
    public static class Program
    {
        private static List<string> IpRanges = new List<string>();

        public static async Task Main()
        {
            FoundDeviceCollection.collection.CollectionChanged += OnChange;

            IpRanges.AddRange(BuildIpRange("192.168.1.1", "192.168.1.254"));

            await BonjourFinder().ConfigureAwait(false);
            await RssdpFinder().ConfigureAwait(false);
            await FindReachableHosts().ConfigureAwait(false);
            await UpnpFinder().ConfigureAwait(false);
            
            //var uPnPTest = new UPnPTest();
        }

        private static async Task UpnpFinder()
        {
            var UpnpSearcher = new UpnpSearcher();
            await UpnpSearcher.BeginSearch().ConfigureAwait(false);
        }

        private static async Task FindReachableHosts()
        {
            var pingHosts = new PingHosts();
            pingHosts.SetIpAddresses(IpRanges);

            await pingHosts.RunPingAsync().ConfigureAwait(false);
        }

        private static void OnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            BuildTable();

            //todo: build out database store.

            /*
            foreach (FoundDevice x in e.NewItems)
            {
                Console.WriteLine($"DEVICENAME: {x.DeviceName}");
                Console.WriteLine($"DEVICEID: {x.DeviceId}");
                Console.WriteLine($"IPADDRESS: {x.IpAddress}");
                Console.WriteLine($"FOUNDAT: {x.FoundAt}");
                Console.WriteLine(new string('.', 80));
            }*/
        }

        private static async Task BonjourFinder()
        {
            try
            {
                var bonjourFinder = new BonjourFinder();
                await bonjourFinder.EnumerateAllServicesFromAllHosts().ConfigureAwait(false);
            }
            catch (System.Net.Http.HttpRequestException)
            {

            }
        }

        private static async Task RssdpFinder()
        {
            try
            {
                var rsdpFinder = new RssdpFinder();
                await rsdpFinder.SearchForDevices().ConfigureAwait(false);
            }
            catch (System.Net.Http.HttpRequestException)
            {

            }
        }

        private static void BuildTable()
        {
            #region Update Page.
            //Console.Clear();
            Console.SetCursorPosition(0, 0);

            List<Tuple<int, string, string, string, string>> devices = new List<Tuple<int, string, string, string, string>>();

            var deviceArray = FoundDeviceCollection.collection.ToArray();
            Array.Sort(deviceArray, ExtensionMethods.IpCompareExtension);

            for (int i = 0; i < deviceArray.Length; i++)
            {
                var deviceID = deviceArray[i].DeviceId ?? "N/A";

                var ip = deviceArray[i].IpAddress;
                var name = deviceArray[i].DeviceName ?? "N/A";

                var id = deviceArray[i].DeviceId;
                var foundUsing = deviceArray[i].FoundUsing;

                var t = Tuple.Create(i, deviceArray[i].IpAddress, name, deviceID, deviceArray[i].FoundUsing);
                devices.Add(t);
            }

            Console.WriteLine(devices.ToStringTable(
                  new[] { "Id", "Ip Address", "Device Name", "Device Id", "Found Using" },
                  a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5));
            #endregion
        }

        private static List<string> BuildIpRange(string start, string end)
        {
            var startAddress = IPAddress.Parse(start);
            var endAddress = IPAddress.Parse(end);

            var ipRange = new RangeFinder();

            var range = ipRange.GetIPRange(startAddress, endAddress);

            return range.Any() ? range.ToList() : new List<string>();
        }
    }
}