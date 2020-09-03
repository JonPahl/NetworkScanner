using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace NetworkScanner.Network
{
    public class Zeroconf
    {
        public Zeroconf()
        {
            //Browse_Click().GetAwaiter().GetResult();
            //ResolveContinuousOnClickListener();
            //Init().GetAwaiter().GetResult();            
            //StartStopListener();
        }


        public async Task<IZeroconfHost?> Search(string protocol, string ipAddress)
        {
            //IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_esp_Hmd._tcp.local.");
            IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync(protocol);

            if (results != null && results.Count > 0)
            {
                var result = results.Where(x => x.IPAddress == ipAddress).FirstOrDefault();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task Init()
        {
            var domains = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);

            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key)).ConfigureAwait(false);
            // var responses = await ZeroconfResolver.ResolveAsync("_http._tcp.local.");

            foreach (var resp in responses)
                WriteLogLine(resp.ToString());
        }

        private async Task Browse_Click()
        {
            var responses = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);

            foreach (var service in responses)
            {
                WriteLogLine(service.Key);

                foreach (var host in service)
                    WriteLogLine("\tIP: " + host);
            }
        }

        private void WriteLogLine(string text, params object[] args)
        {
            Console.WriteLine(string.Format(text, args) + "\r\n");
        }

        protected void OnAnnouncement(ServiceAnnouncement sa)
        {
            WriteLogLine("---- Announced on {0} ({1}) ----", sa.AdapterInformation.Name, sa.AdapterInformation.Address);
            WriteLogLine(sa.Host.ToString());
            WriteLogLine(new string('_', 75));
        }

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
            finally
            {
                // ListenButton.IsChecked = true;
            }
        }

        public async void ResolveContinuousOnClickListener()
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
                    var domains = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);
                    var sub = ZeroconfResolver.ResolveContinuous(domains.Select(g => g.Key));
                    listenSubscription = sub.Subscribe(resp => WriteLogLine(resp.ToString()));
                }
            }
            finally
            {
            }
        }
    }
}

//        public async Task EnumerateAllServicesFromAllHosts()
//        {
//            ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync();
//            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
//            foreach (var resp in responses)
//            {
//                Console.WriteLine(resp.IPAddress);
//                Console.WriteLine(resp.DisplayName);
//                Console.WriteLine(new string('.', 75));
//            }
//        }

//        public async Task Android()
//        {
//            // Somewhere early
//            var wifi = (WifiManager)ApplicationContext.GetSystemService(Context.WifiService);
//            var mlock = wifi.CreateMulticastLock("Zeroconf lock");

//            // Later, before you call Zeroconf
//            try
//            {
//                mlock.Acquire();

//                // Call Zeroconf
//                var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
//                foreach (var resp in responses)
//                {
//                    Console.WriteLine(resp.IPAddress);
//                    Console.WriteLine(resp.DisplayName);
//                    Console.WriteLine(new string('.', 75));
//                }
//            }
//            finally
//            {
//                mlock.Release();
//            }
//        }
//    }
//}