using NetworkScanner.Infrastructure.HostName;
using NetworkScanner.Infrastructure.MDNS;
using System.ComponentModel;

namespace NetworkScanner.Service.Worker
{
    public class MdnsWorker : BackgroundWorker
    {
        private readonly MdnsLookup MdnsLookup;

        public MdnsWorker()
        {
            MdnsLookup = new MdnsLookup(new PingWorkflow());
        }

        public void Run()
        {
            MdnsLookup.QuerySevices();
        }
    }
}
