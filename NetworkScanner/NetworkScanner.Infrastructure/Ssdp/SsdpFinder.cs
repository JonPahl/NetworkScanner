using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using PipleLineExample.Ssdp;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Ssdp
{
    public class SsdpFinder
    {
        private readonly ISsdpDeviceLocator deviceLocator;
        private readonly Auow Uow;

        public SsdpFinder()
        {
            Uow = new RssdpUow();

            deviceLocator = new SsdpDeviceLocator();
            deviceLocator.DeviceAvailable += DeviceLocator_DeviceAvailable;
        }

        /// <summary>
        /// Searches the specified.
        /// </summary>
        /// <param name="term">The term.</param>
        public async Task<IEnumerable<DiscoveredSsdpDevice>> SearchForDevice(string term)
        {
            try
            {
                return await deviceLocator.SearchAsync(term).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsSearching()
        {
            return deviceLocator.IsSearching;
        }

        /// <summary>
        /// Searches the specified ip.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="term">The term.</param>
        public async Task Search() //string ip, string term)
        {
            try
            {
                var foundDevices = await deviceLocator.SearchAsync().ConfigureAwait(false);

                foreach (DiscoveredSsdpDevice device in foundDevices.ToList())
                {
                    SsdpDevice info = await device.GetDeviceInfo().ConfigureAwait(false);

                    var fd = new FoundDevice
                    {
                        IpAddress = device.DescriptionLocation.DnsSafeHost,
                        DeviceName = info.FriendlyName,
                        DeviceId = info.Uuid.Replace("-", ""),
                        FoundUsing = "SSdp",
                        FoundAt = device.AsAt.DateTime
                    };

                    FoundDeviceCollection.Add(fd);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\t{ex.Message}");
                Console.ResetColor();
            }
        }

        private void DeviceLocator_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            try
            {
                var info = e.DiscoveredDevice.GetDeviceInfo().GetAwaiter().GetResult();
                var objects = new List<object> { e.DiscoveredDevice, info };
                var device = Uow.BuildObject<FoundDevice, List<object>>(objects);
                Uow.Commit(device);
            }
            catch (TaskCanceledException)
            {
            }
            catch (HttpRequestException)
            {
            }
        }

        /*
        /// <summary>
        /// Perform a search so we don't have to wait for devices to broadcast notifications
        /// again to get any results right away (notifications are broadcast periodically).
        /// </summary>
        public void StartListening()
        {
            deviceLocator.StartListeningForNotifications();
            deviceLocator.SearchAsync();
        }

        public void StopListening()
        {
            deviceLocator.StopListeningForNotifications();
        }
        */

    }
}
