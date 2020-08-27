using NetworkScanner.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace NetworkScanner.Network
{
    public class BonjourFinder
    {
        public async Task EnumerateAllServicesFromAllHosts()
        {
            var results = await ZeroconfResolver.ResolveAsync("_printer._tcp.local.").ConfigureAwait(false);

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
