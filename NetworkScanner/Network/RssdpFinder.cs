using System;
using System.Threading.Tasks;
using Rssdp;

namespace NetworkScanner.Network
{
    public class RssdpFinder
    {



        public RssdpFinder()
        {

        }

        public async Task<Task> SearchForDevices()
        {
            // This code goes in a method somewhere.
            using var deviceLocator = new SsdpDeviceLocator();
            var foundDevices = await deviceLocator.SearchAsync().ConfigureAwait(false);
            // Can pass search arguments here (device type, uuid). No arguments means all devices.

            foreach (var foundDevice in foundDevices)
            {
                var fullDevice = await foundDevice.GetDeviceInfo().ConfigureAwait(false);

                // Can retrieve the full device description easily though.
                Console.WriteLine("FriendlyName: \t" + fullDevice.FriendlyName);
                Console.WriteLine("FullDeviceType: \t" + fullDevice.FullDeviceType);
                Console.WriteLine("DeviceType: \t" + fullDevice.DeviceType);
                Console.WriteLine("DeviceVersion: \t" + fullDevice.DeviceVersion);
                Console.WriteLine("Ip Address: \t" + foundDevice.DescriptionLocation.Host);
                Console.WriteLine();
                Console.WriteLine("Manufacturer: \t" + fullDevice.Manufacturer);
                Console.WriteLine("ModelName: \t" + fullDevice.ModelName);
                Console.WriteLine("ModelNumber: \t" + fullDevice.ModelNumber);
                Console.WriteLine("SerialNumber: \t" + fullDevice.SerialNumber);
                Console.WriteLine(new string('.', 75));
                Console.WriteLine();
            }
            return Task.CompletedTask;
        }

    }
}
