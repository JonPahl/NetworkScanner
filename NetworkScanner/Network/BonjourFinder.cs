using NetworkScanner.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace NetworkScanner.Network
{
    public class BonjourFinder
    {
        private IDisposable listenSubscription;
        private IObservable<ServiceAnnouncement> subscription;

        public void StartStopListener()
        {
            try
            {
                if (listenSubscription != null)
                {
                    listenSubscription.Dispose();
                    listenSubscription = null;
                }
                else
                {
                    subscription = ZeroconfResolver.ListenForAnnouncementsAsync();
                    listenSubscription = subscription.Subscribe(OnAnnouncement);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnAnnouncement(ServiceAnnouncement sa)
        {
            var DeviceName = FindName(sa);
            var IpAddress = sa.Host.Id ?? "N/A";
            var DeviceId = sa.Host.DisplayName ?? "N/A";

            if (DeviceName != "N/A")
            {
                var fd = new FoundDevice
                {
                    IpAddress = IpAddress,
                    DeviceId = DeviceId,
                    DeviceName = DeviceName,
                    FoundUsing = "Bonjour",
                    FoundAt = DateTime.Now
                };

                FoundDeviceCollection.Add(fd);
            }
        }

        private string FindName(ServiceAnnouncement sa)
        {
            var name = "N/A";
            if (sa.Host.Services?.Count > 0)
            {
                foreach (var prop in sa.Host.Services.FirstOrDefault().Value.Properties)
                {
                    if (prop != null && !prop.TryGetValue("fn", out name))
                    {
                        name = sa.Host.DisplayName;
                        //name = "N/A";
                    }
                }
            }
            return name;
        }

        public async Task EnumerateAllServicesFromAllHosts()
        {
            //var results = await ZeroconfResolver.ResolveAsync("_printer._tcp.local.").ConfigureAwait(false);

            //var type = "_workstation._tcp.";
            //var type = "_printer._tcp.local.";
            //var type = "_services._dns-sd._udp.";//  _services._dns-sd._udp  local.
            //var type = "_ipp._tcp";
            //var type = "hostname.local";
            //var results = await ZeroconfResolver.ResolveAsync(type).ConfigureAwait(false);

            var domains = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);
            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key)).ConfigureAwait(false);
            foreach (var resp in responses)
            {
                var name = "";
                foreach (var prop in resp.Services.FirstOrDefault().Value.Properties)
                {
                    if (!prop.TryGetValue("fn", out name))
                        name = "N/A";
                }

                var fd = new FoundDevice
                {
                    IpAddress = resp.IPAddress,
                    DeviceId = resp.DisplayName,
                    DeviceName = name,
                    FoundUsing = "Bonjour",
                    FoundAt = DateTime.Now
                };

                FoundDeviceCollection.Add(fd);
            }
        }
    }
}
