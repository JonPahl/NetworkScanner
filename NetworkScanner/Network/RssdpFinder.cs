using NetworkScanner.Entities;
using Rssdp;
using System;
using System.Net.Http;

namespace NetworkScanner.Network
{
    /// <summary>
    /// Define _DeviceLocator as a field so it doesn't get GCed after the method ends, and it can
    /// continue to listen for notifications until it is explicitly stopped
    /// (with a call to _DeviceLocator.StopListeningForNotifications();)
    /// </summary>
    public class RssdpFinder
    {
        private readonly SsdpDeviceLocator _DeviceLocator;

        public RssdpFinder()
        {
            _DeviceLocator = new SsdpDeviceLocator();
            //_DeviceLocator.NotificationFilter = "upnp:rootdevice";
        }

        /// <summary>
        /// Starts the listening.
        /// </summary>
        public void StartListening()
        {
            _DeviceLocator.DeviceAvailable += DeviceLocator_DeviceAvailable;
            _DeviceLocator.StartListeningForNotifications();
            _DeviceLocator.SearchAsync();
        }

        public void StopListening()
        {
            _DeviceLocator.StopListeningForNotifications();
        }

        /// <summary>
        /// Handles the DeviceAvailable event of the DeviceLocator control.
        /// Process each found device in the event handler
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DeviceAvailableEventArgs"/> instance containing the event data.</param>
        private async static void DeviceLocator_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            var foundDevice = e.DiscoveredDevice;
            try
            {
                var fullDevice = await foundDevice.GetDeviceInfo().ConfigureAwait(false);

                var localDate = TimeZoneInfo.ConvertTime(e.DiscoveredDevice.AsAt.DateTime, TimeZoneInfo.Local);

                var fd = new FoundDevice
                {
                    DeviceName = fullDevice.FriendlyName,
                    DeviceId = fullDevice.Uuid,
                    IpAddress = fullDevice.ToRootDevice().Location.Host,
                    FoundUsing = "Rssdp",
                    FoundAt = localDate //e.DiscoveredDevice.AsAt.DateTime
                };

                FoundDeviceCollection.Add(fd);
            }
            catch (HttpRequestException)
            {
            }
        }
    }
}