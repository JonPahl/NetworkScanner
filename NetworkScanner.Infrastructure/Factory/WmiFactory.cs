using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.WMI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    /// <summary>
    /// WMI Windows Computer Management Factory
    /// </summary>
    /// <seealso cref="ARpcFactory" />
    public class WmiFactory : ARpcFactory
    {
        private Dictionary<string, string> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiFactory"/> class.
        /// </summary>
        public WmiFactory()
        {
            ServiceName = ServiceEnum.WMI;
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="ip">The IP address.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public override Task<Result> FindValue(string ip, string search)
        {
            var WMI = new WmiManager(ip);

            try
            {
                result = search.ToUpper() switch
                {
                    "SYSTEMNAME" => WMI.FindProperty("Win32_ComputerSystem", new List<string> { "Name" }),
                    "SERIAL" => WMI.FindProperty("Win32_ComputerSystemProduct", new List<string> { "UUID" }),
                    _ => default,
                };

                return (result != null)
                    ? Task.Run(() => new Result() { Value = result.FirstOrDefault().Value, FoundAt = ServiceName.ToString() })
                    : Task.Run(() => new Result() { Value = Utils.Common });
            }
            catch (Exception)
            {
                return Task.Run(() => new Result() { Value = Utils.Common });
            }
        }
    }
}