using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Database;

namespace NetworkScanner.Infrastructure.IpFinder
{
    public class UpdateIpAddresses
    {
        private readonly ScanAddressContext ScanContext;

        public UpdateIpAddresses()
        {
            ScanContext = new ScanAddressContext();
        }

        public bool InsertAddress(ScanAddresses address)
        {
            var result = ScanContext.Insert(address);
            return result > 0;
        }
    }
}
