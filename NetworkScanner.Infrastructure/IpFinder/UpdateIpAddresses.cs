using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Database;

namespace NetworkScanner.Infrastructure.IpFinder
{
    public class UpdateIpAddresses
    {
        private readonly ICrud ScanContext;

        public UpdateIpAddresses()
        {
            ScanContext = new MongoDbContext();
        }

        public bool InsertAddress(ScanAddresses address)
        {
            var result = ScanContext.Insert(address);
            return result > 0;
        }
    }
}
