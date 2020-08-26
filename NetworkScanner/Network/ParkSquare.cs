using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetworkScanner.Entities;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    public class ParkSquare
    {

        public ObservableHashSet<FoundDevice> Devices { get; private set; }

        public ParkSquare()
        {
            Devices = new ObservableHashSet<FoundDevice>();
        }

        public async Task LoadData(string locator)
        {
            ISsdpDeviceLocator deviceLocator = new SsdpDeviceLocator(locator);
            var foundDevices = await deviceLocator.SearchAsync().ConfigureAwait(false);

            //var device = factory.Create(foundDevices.First().DescriptionLocation);

            foreach (var device in foundDevices.ToList())
            {
                Console.WriteLine(device.DescriptionLocation.Host);
                Console.WriteLine(device.DescriptionLocation.HostNameType);

                var info = await device.GetDeviceInfo().ConfigureAwait(false);
                
                var fd = new FoundDevice
                {
                    IpAddress = locator,
                    DeviceName = info.FriendlyName,
                    DeviceId = info.Uuid
                };

                Devices.Add(fd);

                /*
                foreach (var prop in info.GetType().GetProperties().ToList())
                {
                    Console.WriteLine($"\t\t{prop.Name.ToUpper()}:\t{prop.GetValue(info)}");
                }
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
                //    {
                //        Console.WriteLine($"\t\t{action.Name}");
                //    }
                //}
            }
        }

    }
}
