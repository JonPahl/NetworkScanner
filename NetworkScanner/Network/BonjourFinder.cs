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
                Console.WriteLine(resp.IPAddress);
                Console.WriteLine(resp.DisplayName);
                Console.WriteLine("\t"+resp);
            }
        }

    }
}
