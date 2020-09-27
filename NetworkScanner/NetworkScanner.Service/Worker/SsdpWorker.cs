using NetworkScanner.Infrastructure.Ssdp;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NetworkScanner.Service.Worker
{
    public class SsdpWorker : BackgroundWorker
    {
        private readonly SsdpFinder SSDP;

        public SsdpWorker()
        {
            SSDP = new SsdpFinder();
        }

        public async Task RunAsync()
        {
            await SSDP.Search().ConfigureAwait(false);
        }
    }
}