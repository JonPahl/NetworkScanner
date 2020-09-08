using NetworkScanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetworkScanner.Network
{
    public static class BuildDeviceObject
    {
        private static SNMPManager snmp;
        private static WmiManager wmiManager;
        private static MdnsLookup dns;

        public static FoundDevice BuildDevice(string ipAddress, string foundUsing)
        {
            snmp = new SNMPManager();
            wmiManager = new WmiManager(ipAddress);
            dns = new MdnsLookup();

            var systemName = FindDeviceName(ipAddress);
            var Uuid = FindDeviceId(ipAddress);

            var fd = new FoundDevice
            {
                IpAddress = ipAddress,
                DeviceId = Uuid,
                DeviceName = systemName,
                FoundUsing = foundUsing,
                FoundAt = DateTime.Now
            };

            return fd;
        }

        private static string FindDeviceName(string ip)
        {
            var systemName = "N/A";
            systemName = FindHostEntry(ip);

            if (systemName == "N/A")
            {
                var service = "_esp_temp._tcp.local"; //todo: pass this in.
                systemName = DisplayName(ip, service);
            }

            if (systemName == "N/A")
            {
                systemName = snmp.FindValueInAllCommunitities(ip, "1.3.6.1.2.1.1.5.0");
                if (string.IsNullOrEmpty(systemName))
                    systemName = "N/A";
            }

            return systemName;
        }


        #region findDeviceNameLookup

        private static string FindHostEntry(string ip)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                return (hostEntry != null && !string.IsNullOrWhiteSpace(hostEntry.HostName)) ? hostEntry.HostName : "N/A";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        private static string DisplayName(string ipAddress, string service)
        {
            try
            {
                var records = dns.ServiceInstanceAsync(service).GetAwaiter().GetResult();
                var result = records[ipAddress];
                return result ?? "N/A";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        #endregion



        #region Find Device UUID

        private static string FindDeviceId(string ip)
        {
            var DeviceId = "N/A"; // $"{ip}_NA";

            var compSysProd = wmiManager.FindProperty("Win32_ComputerSystemProduct", new List<string> { "UUID" });
            DeviceId = (compSysProd != null) ? compSysProd.FirstOrDefault().Value : "N/A"; // $"{ip}_NA";

            if (DeviceId == "N/A") // $"{ip}_NA")
            {
                var service = "_esp_temp._tcp.local"; //todo: pass this in.
                DeviceId = DisplayId(ip, service);
            }
            return DeviceId;
        }

        private static string DisplayId(string ipAddress, string service)
        {
            try
            {
                var records = dns.ServiceInstanceAsync(service).GetAwaiter().GetResult();
                var result = records[ipAddress];
                return result ?? "N/A";// $"{ipAddress}_N/A";
            }
            catch (Exception)
            {
                return "N/A";
                //return $"{ipAddress}_N/A";
            }
        }

        #endregion
    }
}
