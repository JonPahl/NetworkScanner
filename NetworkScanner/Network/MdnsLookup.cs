using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.Text;

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


            sd.ServiceDiscovered += Sd_ServiceDiscovered;
            sd.ServiceInstanceDiscovered += Sd_ServiceInstanceDiscovered;
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