using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkScanner.Network
{
    public class MdnsLookup
    {
        private ServiceDiscovery sd;
        private MulticastService mdns;

        public MdnsLookup()
        {
            mdns = new MulticastService();
            sd = new ServiceDiscovery();

            //sd.ServiceDiscovered += Sd_ServiceDiscovered;
            //sd.ServiceInstanceDiscovered += Sd_ServiceInstanceDiscovered;
        }

        /// <summary>
        /// Services the instance asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ServiceInstanceAsync(string service)
        {
            var results = new Dictionary<string, string>();
            var delay = new TimeSpan(0, 1, 0);
            var cancellation = new CancellationTokenSource(delay);
            try
            {
                var query = new Message();
                query.Questions.Add(new Question { Name = service, Type = DnsType.ANY });

                using var mdns = new MulticastService();
                mdns.Start();
                var response = await mdns.ResolveAsync(query, cancellation.Token).ConfigureAwait(false);

                foreach (AddressRecord answer in response.AdditionalRecords.Where(x => x.Type.Equals(DnsType.A)))
                {
                    results[answer.Address.ToString()] = answer.Name.Labels[0];
                }
                mdns.Stop();
                return results;
            }
            catch (TaskCanceledException ex)
            {
                return results;
            }
        }


        private void Sd_ServiceInstanceDiscovered(object sender, ServiceInstanceDiscoveryEventArgs e)
        {
            List<ResourceRecord> answers = e.Message.Answers;

            foreach (var msg in answers)
            {
                Console.WriteLine(msg.CanonicalName);
                Console.WriteLine(msg.CreationTime);                
            }
            ////throw new NotImplementedException();
        }

        private void Sd_ServiceDiscovered(object sender, DomainName e)
        {
            throw new NotImplementedException();
        }


    }
}