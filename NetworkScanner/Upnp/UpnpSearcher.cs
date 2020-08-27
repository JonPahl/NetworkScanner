using NetworkScanner.Entities;
using Rssdp;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace NetworkScanner.Upnp
{
    public class UpnpSearcher
    {
        // Define _DeviceLocator as a field so it doesn't get GCed after the method ends, and it can
        // continue to listen for notifications until it is explicitly stopped 
        // (with a call to _DeviceLocator.StopListeningForNotifications();)
        private SsdpDeviceLocator _DeviceLocator;

        private ObservableCollection<SsdpDevice> foundDevices;

        public UpnpSearcher()
        {
            foundDevices = new ObservableCollection<SsdpDevice>();
            foundDevices.CollectionChanged += OnChange;
        }

        private void OnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach(SsdpDevice item in e.NewItems)
            {
                var fd = new FoundDevice
                {
                    DeviceId = item.Uuid,
                    DeviceName = item.FriendlyName,
                    FoundUsing = "Upnp Searcher",
                    IpAddress = item.ToRootDevice().Location.Host
                };

                FoundDeviceCollection.collection.Add(fd);
            }
        }


        // Call this method from somewhere in your code to start the search.
        public async Task BeginSearch()
        {
            // (Optional) Set the filter so we only see notifications for devices we care about 
            // (can be any search target value i.e device type, uuid value etc - any value that appears in the 
            // DiscoverdSsdpDevice.NotificationType property or that is used with the searchTarget parameter of the Search method).

            _DeviceLocator = new SsdpDeviceLocator
            {
                NotificationFilter = "upnp:rootdevice"
            };
            _DeviceLocator.DeviceAvailable += DeviceLocator_DeviceAvailable;

            // Enable listening for notifications (optional)
            _DeviceLocator.StartListeningForNotifications();

            TimeSpan time = new TimeSpan(0, 2, 0);
            // Perform a search so we don't have to wait for devices to broadcast notifications 
            // again to get any results right away (notifications are broadcast periodically).
             _DeviceLocator.SearchAsync(time);
            
            Console.ReadLine();
        }

        // Process each found device in the event handler
        private async void DeviceLocator_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            try
            {
                var fullDevice = await e.DiscoveredDevice.GetDeviceInfo().ConfigureAwait(false);
                var fd = new FoundDevice
                {
                    FoundAt = e.DiscoveredDevice.AsAt.DateTime,
                    DeviceId = fullDevice.Uuid,
                    DeviceName = fullDevice.FriendlyName,
                    FoundUsing = "Upnp Searcher",
                    IpAddress = fullDevice.ToRootDevice().Location.Host
                };

                FoundDeviceCollection.collection.Add(fd);
            }
            catch (System.Net.Http.HttpRequestException)
            {

            }
        }

    }
}
