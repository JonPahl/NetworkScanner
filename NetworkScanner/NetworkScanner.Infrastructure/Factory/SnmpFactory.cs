using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.SNMP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    /// <summary>
    /// SNMP related calls.
    /// </summary>
    /// <seealso cref="ARpcFactory" />
    public class SnmpFactory : ARpcFactory
    {
        private readonly List<KeyValuePair<string, string>> values;
        private readonly SNMPManager SNMP;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpFactory"/> class.
        /// </summary>
        public SnmpFactory()
        {
            SNMP = new SNMPManager();
            ServiceName = ServiceEnum.SNMP;
            values = Utils.LoadSNMPOptions();
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public override Task<Result> FindValue(string ip, string search)
        {
            try
            {
                var oid = values.Find(x => x.Key.Equals(search)).Value ?? string.Empty;

                var value = SNMP.FindValueInAllCommunitities(ip, oid);

                return !string.IsNullOrEmpty(oid)
                    ? Task.Run(() => new Result { Value = value, FoundAt = ServiceName.ToString() })
                    : Task.Run(() => new Result { Value = Utils.Common });
            } catch(Exception)
            {
                return Task.Run(() => new Result() { Value = Utils.Common });
            }
        }
    }
}
