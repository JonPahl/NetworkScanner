using NetworkScanner.Entities;
using NetworkScanner.Network;
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
        public static async Task Main()
        {
            var IpRanges = new List<string>();
            IpRanges.AddRange(BuildIpRange("192.168.1.1", "192.168.1.255"));
           
            try
            {
                foreach (var ip in IpRanges)
                {
                    var isUsed = await PingAble.PingHost(ip).ConfigureAwait(false);

                    if (!isUsed)
                        continue;

                    try
                    {
                        var park = new Network.ParkSquare();
                        park.Devices.CollectionChanged += OnChange;

                        await park.LoadData(ip).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                Console.WriteLine(new string('=', 75));

                await BonjourFinder().ConfigureAwait(false);

                Console.WriteLine(new string('=', 75));

                await RssdpFinder().ConfigureAwait(false);


                var mig = new UPnP();

            }
            catch (Exception ex)
            {

            }
        }

        private static void OnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach(FoundDevice x in e.NewItems)
            {
                Console.WriteLine(x.DeviceName);
                Console.WriteLine(x.DeviceId);
                Console.WriteLine(x.IpAddress);
                Console.WriteLine(new string('.', 80));
            }

            //throw new NotImplementedException();
        }

        private static async Task BonjourFinder()
        {
            var bonjourFinder = new BonjourFinder();
            await bonjourFinder.EnumerateAllServicesFromAllHosts().ConfigureAwait(false);
        }

        private static async Task RssdpFinder()
        {
            var rsdpFinder = new RssdpFinder();
            await rsdpFinder.SearchForDevices().ConfigureAwait(false);
        }

        private static List<string> BuildIpRange(string start, string end)
        {
            var startAddress = IPAddress.Parse(start);
            var endAddress = IPAddress.Parse(end);

            var ipRange = new RangeFinder();

            var range = ipRange.GetIPRange(startAddress, endAddress);

            return (range.Any()) ? range.ToList() : new List<string>();
        }
    }
}


/*
            foreach (var ip in IpRanges)
            {
                var isUsed = await PingAble.PingHost(ip).ConfigureAwait(false);

                if (isUsed)
                {
                    try
                    {
                        var park = new Network.ParkSquare();
                        park.Devices.CollectionChanged += OnChange;

                        await park.LoadData(ip).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            */

/*
foreach (var ip in IpRanges)
{
    var isUsed = await PingAble.PingHost(ip).ConfigureAwait(false);

    if (isUsed)
    {
        var snmp = new SNMPManager(ip, "community");
        var wmiManager = new WmiManager(ip);

        var results = wmiManager.FindProperty("Win32_OperatingSystem", new List<string> { "CSName" });
        // Win32_OperatingSystem // CSName


        var systemName = snmp.FindValue("1.3.6.1.2.1.1.5.0");
        var upTime = snmp.FindValue("1.3.6.1.2.1.1.3.0");

        Console.WriteLine(ip);
        Console.WriteLine($"\t\t{isUsed}");
        Console.WriteLine($"\t\t{systemName}");
        Console.WriteLine($"\t\t{upTime}");
    }
}
Console.WriteLine(new string('-', 75));
*/

/*
       foreach (var ip in IpRanges)
       {
           var isUsed = await PingAble.PingHost(ip).ConfigureAwait(false);

           if (isUsed)
           {
               var systemName = PingAble.GetMachineNameFromIPAddress(ip);

               Console.WriteLine(ip);                        
               Console.WriteLine($"\t\t{isUsed}");
               Console.WriteLine($"\t\t{systemName}");
           }
       }
   */