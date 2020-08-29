using NetworkScanner.Entities;
using NetworkScanner.Network;
using NetworkScanner.Upnp;
using PEFile;
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
        private static BonjourFinder bonjourFinder;
        private static UpnpSearcher UpnpSearcher;
        private static RssdpFinder rsdpFinder;
        private static SsdpFinder ssdp;

        public static async Task Main()
        {
            FoundDeviceCollection.Changed += CollectionChanged;
            IpRanges.AddRange(BuildIpRange("192.168.1.1", "192.168.1.254"));

            #region Setup
            bonjourFinder = new BonjourFinder();
            UpnpSearcher = new UpnpSearcher();
            rsdpFinder = new RssdpFinder();
            ssdp = new SsdpFinder();
            #endregion

            #region Start Listening
            await ssdp.LoadData("192.168.1.250").ConfigureAwait(false);

            bonjourFinder.StartStopListener();
            await UpnpSearcher.BeginSearch().ConfigureAwait(false);
            await FindReachableHostsAsync().ConfigureAwait(false);
            rsdpFinder.StartListening();
            //RssdpFinder();
            //await UpnpFinderAsync();

            // https://youtu.be/4QGb2wMrCsk?t=15049
            #endregion

            //var uPnPTest = new UPnPTest();
            //uPnPTest.StartHere();

            /*
            BonjourFinder();
            RssdpFinder();
            await FindReachableHostsAsync().ConfigureAwait(false);
            await UpnpFinderAsync()
            //    .ContinueWith(_ =>
            //    {
            //        uPnPTest.StartHere();
            //    })
                .ConfigureAwait(false);
            */

            Console.ReadLine();
        }

        private static void CollectionChanged(object sender, FoundDeviceChangedEventArgs e)
        {
            BuildTable();
        }

        private static async Task FindReachableHostsAsync()
        {
            var pingHosts = new PingHosts();
            pingHosts.SetIpAddresses(IpRanges);

            await pingHosts.RunPingAsync().ConfigureAwait(false);
        }

        /*
        private static void BonjourFinder()
        {
            try
            {
                //var bonjourFinder = new BonjourFinder();
                //await bonjourFinder.EnumerateAllServicesFromAllHosts().ConfigureAwait(false);

                var bonjourFinder = new BonjourFinder();
                bonjourFinder.StartStopListener();
            }
            catch (System.Net.Http.HttpRequestException)
            {

            }
        }
        */

        private static void RssdpFinder()
        {
            try
            {
                var rsdpFinder = new RssdpFinder();
                rsdpFinder.StartListening();
                //await rsdpFinder.SearchForDevices().ConfigureAwait(false);
            }
            catch (System.Net.Http.HttpRequestException)
            {
            }
        }

        private static IList<string> BuildIpRange(string start, string end)
        {
            var startAddress = IPAddress.Parse(start);
            var endAddress = IPAddress.Parse(end);
            var ipRange = new RangeFinder();
            var range = ipRange.GetIPRange(startAddress, endAddress);

            return range.Any() ? range.ToList() : new List<string>();
        }

        #region Process results

        private static void BuildTable()
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

        private static string DisplayTable(List<Tuple<int, string, string, string, string, string>> devices)
        {
            return devices.ToStringTable(
                              new[] { "Id", "Ip Address", "Device Name", "Device Id", "Found Using", "TimeStamp" },
                              a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6);
        }

        #endregion

    }
}