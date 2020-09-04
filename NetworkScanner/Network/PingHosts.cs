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
    public class PingHosts
    {
        private List<string> ipAddresses = new List<string>();

        private readonly int timeout = 5000;
        private int nFound = 0;

        private static object lockObj = new object();
        private Stopwatch stopWatch = new Stopwatch();
        private TimeSpan ts;

        private Zeroconf Zeroconf;
        //private SsdpFinder SSDP;  //ssdp.StartListening();

        public PingHosts()
        {
            Zeroconf = new Zeroconf();
            //SSDP = new SsdpFinder();
        }

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
                /*
                var ssdpResult = await SSDP.SearchForDevice(ip, "upnp:rootdevice");
                while(SSDP.IsSearching())
                    Thread.Sleep(500);
                */

                var deviceName = FindDeviceName(ip);
                var deviceId = FindDeviceId(ip);

                var fd = new FoundDevice
                {
                    IpAddress = ip,
                    DeviceName = deviceName,
                    DeviceId = deviceId,
                    FoundAt = DateTime.Now,
                    FoundUsing = "Ping"
                };

                FoundDeviceCollection.Add(fd);

                lock (lockObj)
                {
                    nFound++;
                }
            }
        }

        private string FindDeviceName(string ip)
        {
            var systemName = "N/A";

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                if (hostEntry != null && !string.IsNullOrWhiteSpace(hostEntry.HostName))
                {
                    systemName = hostEntry.HostName;
                }
            }
            catch (Exception ex)
            {
                systemName = "N/A";
            }

            if (systemName == "N/A")
            {
                var service = "_esp_temp._tcp.local"; //todo: pass this in.
                systemName = DisplayName(ip, service);
            }

            if (systemName == "N/A")
            {
                var snmp = new SNMPManager();
                systemName = snmp.FindValueInAllCommunitities(ip, "1.3.6.1.2.1.1.5.0");

                if (string.IsNullOrEmpty(systemName))
                    systemName = "N/A";
            }

            return systemName;
        }


        private string FindDeviceId(string ip)
        {
            var DeviceId = $"{ip}_NA";

            var wmiManager = new WmiManager(ip);
            var compSysProd = wmiManager.FindProperty("Win32_ComputerSystemProduct", new List<string> { "UUID" });
            DeviceId = (compSysProd != null) ? compSysProd.FirstOrDefault().Value : $"{ip}_NA";

            /*
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                if (hostEntry != null && !string.IsNullOrWhiteSpace(hostEntry.HostName))
                {
                    systemName = hostEntry.HostName;
                }
            }
            catch (Exception ex)
            {
                systemName = $"{ip}_NA";
            }
            */

            if (DeviceId == $"{ip}_NA")
            {
                var service = "_esp_temp._tcp.local"; //todo: pass this in.
                DeviceId = DisplayId(ip, service);
            }
            /*if (systemName == "N/A"){foreach (var community in Utils.LoadCommunities()){
                    var snmp = new SNMPManager(ip, community);systemName = snmp.FindValue("1.3.6.1.2.1.1.5.0");
                    if (string.IsNullOrEmpty(systemName))systemName = $"{ip}_NA";break;
                }}*/
            return DeviceId;
        }

        private string DisplayId(string ipAddress, string service)
        {
            try
            {
                //var service = "_esp_temp._tcp.local"; //todo: pass this in.
                var dns = new MdnsLookup();

                var records = dns.ServiceInstanceAsync(service).GetAwaiter().GetResult();
                var result = records[ipAddress];

                return result ?? $"{ipAddress}_N/A";
            }
            catch (Exception)
            {
                return $"{ipAddress}_N/A";
            }
        }

        private string DisplayName(string ipAddress, string service)
        {
            try
            {
                var dns = new MdnsLookup();

                var records = dns.ServiceInstanceAsync(service).GetAwaiter().GetResult();
                var result = records[ipAddress];

                return result ?? "N/A";
            }
            catch (Exception ex)
            {
                return "N/A";
            }
        }
    }
}
