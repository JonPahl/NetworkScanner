using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    /// <summary>
    /// Check for DNS HostName.
    /// </summary>
    /// <seealso cref="ARpcFactory" />
    public class DeviceNameFactory : ARpcFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceNameFactory"/> class.
        /// </summary>
        public DeviceNameFactory()
        {
            ServiceName = ServiceEnum.DEVICENAME;
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="ip">The IP Address</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public override Task<Result> FindValue(string ip, string search)
            => (search.ToUpper()) switch
            {
                "SYSTEMNAME" => DoLookup(ip),
                _ => Task.Run(() => new Result { Value = Utils.Common }),
            };

        private Task<Result> DoLookup(string ip)
        {
            try
            {
                var DnsHostName = Dns.GetHostEntry(ip);

                return DnsHostName != null && !string.IsNullOrEmpty(DnsHostName.HostName)
                    ? Task.Run(() => new Result { Value = DnsHostName.HostName, FoundAt = ServiceName.ToString() })
                    : Task.Run(() => new Result { Value = Utils.Common, FoundAt = ServiceName.ToString() });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.Run(() => new Result { Value = Utils.Common, FoundAt = ServiceName.ToString() });
            }
        }
    }
}