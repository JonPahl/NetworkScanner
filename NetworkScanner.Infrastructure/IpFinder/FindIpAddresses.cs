using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Database;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetworkScanner.Infrastructure.IpFinder
{
    /// <summary>
    /// Query database for stored IP Addresses and then convert a range to individual values.
    /// </summary>
    public class FindIpAddresses
    {
        private readonly RangeFinder IpRange;
        public List<string> IpAddresses;
        /// private readonly ScanAddressContext ScanContext;
        private ICrud ScanContext { get; set; }
        public FindIpAddresses(ICrud context)
        {
            IpRange = new RangeFinder();
            IpAddresses = new List<string>();
            ScanContext = context; //new ScanAddressContext();
            // ElasticSearchContext
        }

        /// <summary>
        /// Load starting and ending IP addresses
        /// </summary>
        /// <returns>List of Scan Addresses</returns>
        public IList<ScanAddresses> LoadAddresses
        {
            get
            {
                var addresses = ScanContext.LoadAllAsync<ScanAddresses>().GetAwaiter().GetResult();
                return addresses.FindAll(x => x.IsActive.Equals(true));
                //return ScanContext.LoadAll<ScanAddresses>().FindAll(x => x.IsActive.Equals(true));
            }
        }

        /// <summary>
        /// Convert list of starting and ending IP addresses to list of addresses.
        /// </summary>
        public void BuildIpRanges(List<ScanAddresses> IpRanges)
        {
            foreach (ScanAddresses ip in IpRanges.Distinct())
            {
                var range = IpRange.GetIPRange(
                    IPAddress.Parse(ip.StartAddress),
                    IPAddress.Parse(ip.EndAddress)).ToList();

                IpAddresses.AddRange(range);
            }
        }
    }
}
