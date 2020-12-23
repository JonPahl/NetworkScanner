using Makaretu.Dns;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    public class MdnsFactory //: ARpcFactory
    {
        private AddressRecord ARecord;
        private TXTRecord TRecord;
        private readonly string ServiceName;
        public MdnsFactory()
        {
            ServiceName = nameof(ServiceEnum.MDNS);
            TRecord = null;
            ARecord = null;
        }

        public Task<Result> FindValue(string ip, string search)
        {
            var services = Utils.LoadMdnsServices();
            var result = new Result { FoundAt = ServiceName };

            try
            {
                foreach (var service in services)
                {
                    try
                    {
                        Message msg = Lookup(service).GetAwaiter().GetResult();
                        if (msg != null)
                        {
                            var random = new Random();

                            #region Store result                    
                            ResourceRecord record = msg.Answers.FirstOrDefault();

                            if (msg.AdditionalRecords != null)
                            {
                                ARecord = (AddressRecord)msg
                                    .AdditionalRecords.Find(x => x.Type.Equals(DnsType.A));
                                TRecord = (TXTRecord)msg
                                    .AdditionalRecords.Find(x => x.Type.Equals(DnsType.TXT));

                                var DeviceName = Utils.Common;
                                var DeviceId = Utils.Common;

                                if (ARecord?.Address.ToString().Equals(ip) == true)
                                {
                                    DeviceId = ARecord.Name.Labels
                                        .FirstOrDefault()?.Replace("-", "") ?? Utils.Common;
                                    DeviceName = ARecord.Name.Labels
                                        .FirstOrDefault() ?? Utils.Common;
                                }

                                if (TRecord != null)
                                {
                                    var prop = TRecord.Strings.Find(x => x.Contains("fn"));
                                    if (prop != null)
                                    {
                                        var info = prop.Split("=");
                                        DeviceName = info[0];
                                    }
                                }

                                switch (search.ToUpper())
                                {
                                    case "SYSTEMNAME":
                                        result.Value = DeviceName;
                                        break;
                                    case "SERIAL":
                                        result.Value = DeviceId;
                                        break;
                                }

                                if (ARecord != null || TRecord != null)
                                {
                                    // Uow.Commit(foundDevice); 
                                    return Task.Run(() => result);
                                }
                            }
                            else
                            {
                                // ToDo: what should I do then...
                            }
                            #endregion                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return Task.Run(() => new Result() { Value = Utils.Common });
            }
            catch (Exception)
            {
                return Task.Run(() => new Result() { Value = Utils.Common });
            }
        }

        private async Task<Message> Lookup(string service)
        {
            try
            {
                var query = new Message();
                query.Questions.Add(new Question { Name = service, Type = DnsType.ANY });
                var cancellation = new CancellationTokenSource(10000);

                using MulticastService mdns = new MulticastService();
                mdns.AnswerReceived += Mdns_AnswerReceived;

                mdns.Start();
                Message response = await mdns.ResolveAsync(query, cancellation.Token).ConfigureAwait(false);
                mdns.Stop();
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void Mdns_AnswerReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
}
