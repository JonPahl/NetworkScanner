using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Upnp
{
    public class UpnpSearcher
    {
        private ISsdpDeviceLocator deviceLocator;
        private readonly Auow auow;
        public UpnpSearcher()
        {
            deviceLocator = new SsdpDeviceLocator();
            deviceLocator.DeviceAvailable += DeviceLocator_DeviceAvailable;

            auow = new UpnpUow();
        }

        public void StartListening()
        {
            deviceLocator.StartListeningForNotifications();
        }
        public void StopListening()
        {
            deviceLocator.StopListeningForNotifications();
        }

        private void DeviceLocator_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            try
            {
                var device = e.DiscoveredDevice;
                var infoDevice = device.GetDeviceInfo().ConfigureAwait(false).GetAwaiter().GetResult();

                var objs = new List<object> { device, infoDevice };
                var foundDevice = auow.BuildObject<FoundDevice, List<object>>(objs);

                FoundDeviceCollection.Add(foundDevice);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<DiscoveredSsdpDevice>> Search()
        {
            deviceLocator = new SsdpDeviceLocator();

            try
            {
                var foundDevices = deviceLocator.SearchAsync().Result.ToList();

                if (foundDevices.Count > 0)
                {
                    foreach (var device in foundDevices)
                    {
                        SsdpDevice info = await device.GetDeviceInfo().ConfigureAwait(false);

                        var objs = new List<object> { device, info };

                        var foundDevice = auow.BuildObject<FoundDevice, List<object>>(objs);
                        auow.Commit(foundDevice);
                    }
                }

                return foundDevices.ToList();
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}