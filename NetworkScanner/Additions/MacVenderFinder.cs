using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using ArpLookup;


namespace NetworkScanner.Additions
{
public class MacVenderFinder
    {
        private MacAddressVendorLookup.MacVendorBinaryReader vendorInfoProvider = new MacAddressVendorLookup.MacVendorBinaryReader();

        public MacVenderFinder()
        {
            vendorInfoProvider = new MacAddressVendorLookup.MacVendorBinaryReader();
           
        }

        public async Task<string> GetMacByIP(string ipAddress)
        {
            try
            {
                PhysicalAddress mac = await Arp.LookupAsync(IPAddress.Parse(ipAddress));
                return mac.ToString();
            }
            catch (Exception)
            {
                return "N/A";
            }            
        }

        public string Lookup(PhysicalAddress mac)
        {
            using (var resourceStream = MacAddressVendorLookup.ManufBinResource.GetStream().Result)
            {
                vendorInfoProvider.Init(resourceStream).Wait();
            }
            var addressMatcher = new MacAddressVendorLookup.AddressMatcher(vendorInfoProvider);

            
            var vendorInfo = addressMatcher.FindInfo(mac);
            if(vendorInfo != null)
            {
                return vendorInfo.IdentiferString;
            } else
            {
                return "N/A";
            }
            //Console.WriteLine("\nAdapter: " + ni.Description);
            //Console.WriteLine($"\t{vendorInfo}");
            //var macAddr = BitConverter.ToString(mac.GetPhysicalAddress().GetAddressBytes()).Replace('-', ':');
            //Console.WriteLine($"\tMAC Address: {macAddr}");

            //return vendorInfo.IdentiferString;
        }

    }
}
