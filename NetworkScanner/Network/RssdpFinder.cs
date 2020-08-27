using System.Threading.Tasks;
using NetworkScanner.Entities;
using Rssdp;

namespace NetworkScanner.Network
{
    public class RssdpFinder
    {
        public async Task<Task> SearchForDevices()
        {
            using var deviceLocator = new SsdpDeviceLocator();
            var foundDevices = await deviceLocator.SearchAsync().ConfigureAwait(false);
            // Can pass search arguments here (device type, uuid). No arguments means all devices.

            foreach (var foundDevice in foundDevices)
            {
                var fullDevice = await foundDevice.GetDeviceInfo().ConfigureAwait(false);

                var fd = new FoundDevice
                {
                    DeviceName = fullDevice.FriendlyName,
                    DeviceId = fullDevice.Uuid,
                    IpAddress = fullDevice.ToRootDevice().Location.Host,
                    FoundUsing = "Rssdp",
                };

                FoundDeviceCollection.Add(fd);
            }
            return Task.CompletedTask;
        }
    }
}
