using Makaretu.Dns;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using System;
using System.Threading;

namespace NetworkScanner.Infrastructure.MDNS
{
    /// <summary>
    /// Lookup for MDNS
    /// </summary>
    public class MdnsLookup
    {
        private readonly MulticastService mdns;
        private readonly ServiceDiscovery sd;
        private readonly Auow Uow;

        public MdnsLookup(Auow workflow)
        {
            Uow = workflow;
            mdns = new MulticastService();
            mdns.AnswerReceived += Mdns_AnswerReceived;

            sd = new ServiceDiscovery(mdns);
            sd.ServiceInstanceDiscovered += Sd_ServiceInstanceDiscovered;
        }

        public void StartListening() => mdns.Start();
        public void StopListening() => mdns.Stop();
        private void Sd_ServiceInstanceDiscovered(object sender, ServiceInstanceDiscoveryEventArgs e) => ParseMessage(e.Message);
        private void Mdns_AnswerReceived(object sender, MessageEventArgs e) => ParseMessage(e.Message);

        public void QuerySevices()
        {
            CancellationTokenSource cancellation
                = new CancellationTokenSource(new TimeSpan(0, 0, 30));
            StartListening();

            foreach (var service in Utils.LoadMdnsServices())
            {
                try
                {
                    var query = new Message();
                    query.Questions.Add(new Question { Name = service, Type = DnsType.ANY, CreationTime = DateTime.Now });
                    Message message = mdns.ResolveAsync(query, cancellation.Token).GetAwaiter().GetResult();
                    ParseMessage(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            StopListening();
        }

        private void ParseMessage(Message message)
        {
            foreach (var record in message.Answers)
            {
                if (message?.AdditionalRecords.Count > 0)
                {
                    AddressRecord ARecord = (AddressRecord)message.AdditionalRecords.Find(x => x.Type.Equals(DnsType.A));
                    TXTRecord TRecord = (TXTRecord)message.AdditionalRecords.Find(x => x.Type.Equals(DnsType.TXT));

                    var services = Utils.LoadMdnsServices();
                    var name = record.Name.ToString();
                    if (services.Contains(name) && ARecord != null)
                    {
                        var foundDevice = BuildResponse(ARecord, TRecord);
                        Uow.Commit(foundDevice);
                    }
                }
            }
        }

        private FoundDevice BuildResponse(AddressRecord aRecord, TXTRecord tXTRecord)
        {
            var IpAddress = aRecord.Address.ToString();
            var DeviceId = aRecord.Name.Labels[0].Replace("-", "");
            var DeviceName = Utils.Common;

            if (tXTRecord != null)
            {
                var prop = tXTRecord.Strings.Find(x => x.Contains("fn"));
                if (prop != null)
                {
                    var info = prop.Split("=");
                    DeviceName = info[1] ?? Utils.Common;
                }
                else
                {
                    DeviceName = aRecord.Name.Labels[0];
                }
            }
            else
            {
                DeviceName = aRecord.Name.Labels[0];
            }

            return new FoundDevice
            {
                IpAddress = IpAddress,
                DeviceId = DeviceId,
                DeviceName = DeviceName,
                FoundAt = aRecord.CreationTime,
                FoundUsing = "MDNS"
            };
        }
    }
}