using NetworkScanner.Entities;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    public class SsdpFinder
    {
        /// <summary>
        /// Searches the specified ip.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="term">The term.</param>
        public async Task Search(string ip, string term)
        {
            try
            {
                ISsdpDeviceLocator deviceLocator = new SsdpDeviceLocator();
                var foundDevices = await deviceLocator.SearchAsync(term).ConfigureAwait(false);

                foreach (var device in foundDevices.ToList())
                {
                    var info = await device.GetDeviceInfo().ConfigureAwait(false);

                    var fd = new FoundDevice
                    {
                        IpAddress = ip,
                        DeviceName = info.FriendlyName,
                        DeviceId = info.Uuid,
                        FoundUsing = "SSdp"
                    };

                    FoundDeviceCollection.Add(fd);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\t{ex.Message}");
                Console.ResetColor();
                //BuildObject(ip);
            }
        }

        public async Task LoadData(string ip)
        {
            try
            {
                ISsdpDeviceLocator deviceLocator = new SsdpDeviceLocator();
                var foundDevices = await deviceLocator.SearchAsync(ip).ConfigureAwait(false);

                if (foundDevices.Count() == 0)
                {
                    BuildObject(ip);
                }
                else
                {
                    foreach (var device in foundDevices.ToList())
                    {
                        var info = await device.GetDeviceInfo().ConfigureAwait(false);

                        var fd = new FoundDevice
                        {
                            IpAddress = ip,
                            DeviceName = info.FriendlyName,
                            DeviceId = info.Uuid,
                            FoundUsing = "SSdp",
                            FoundAt = DateTime.Now //info.ToRootDevice().
                        };

                        FoundDeviceCollection.Add(fd);
                    }
                }
            }
            catch (Exception)
            {
                BuildObject(ip);
            }
        }

        public void BuildObject(string ip)
        {
            var snmp = new SNMPManager(ip, "community");
            var wmiManager = new WmiManager(ip);

            var systemName = snmp.FindValue("1.3.6.1.2.1.1.5.0");
            //var upTime = snmp.FindValue("1.3.6.1.2.1.1.3.0");

            if (string.IsNullOrEmpty(systemName))
            {
                var OsResult = wmiManager.FindProperty("Win32_OperatingSystem", new List<string> { "CSName" });
                systemName = (OsResult != null) ? OsResult.FirstOrDefault().Value : "N/A";
            }

            if (systemName == "N/A")
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                    systemName = hostEntry.HostName;
                } catch(Exception ex)
                {
                    var x = ex.Message;
                }
            }

            string Uuid = null;
            var ProductResult = wmiManager.FindProperty("Win32_ComputerSystemProduct", new List<string> { "UUID" });
            Uuid = (ProductResult?.FirstOrDefault().Value);

            var fd = new FoundDevice
            {
                IpAddress = ip,
                DeviceId = Uuid,
                DeviceName = systemName,
                FoundUsing = "Ping",
                FoundAt = DateTime.Now
            };

            FoundDeviceCollection.Add(fd);
        }
    }
}

/*
foreach (var prop in info.GetType().GetProperties().ToList())
    Console.WriteLine($"\t\t{prop.Name.ToUpper()}:\t{prop.GetValue(info)}");
*/

//Console.WriteLine(info.FriendlyName);
//Console.WriteLine(info.DeviceType);
//Console.WriteLine(info.DeviceVersion);
//Console.WriteLine(info.Manufacturer);
//Console.WriteLine(info.ModelNumber);
//Console.WriteLine(info.SerialNumber);
//Console.WriteLine();
//Console.WriteLine(new string('*', 75));
//Console.WriteLine();
//Console.WriteLine(device.ToString());
//foreach (var service in device.Services)
//{
//    Console.WriteLine($"\t{service.ServiceId} ({service.ServiceType})");
//    foreach (var action in service.Actions)
//        Console.WriteLine($"\t\t{action.Name}");
//}