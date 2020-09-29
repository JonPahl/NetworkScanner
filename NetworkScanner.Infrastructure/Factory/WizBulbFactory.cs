using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Domain.LightBulb;
using System;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    /// <summary>
    /// Wiz Light Bulb
    /// </summary>
    /// <seealso cref="ARpcFactory" />
    public class WizBulbFactory : ARpcFactory
    {
        public WizLightBulb bulb;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizBulbFactory"/> class.
        /// </summary>
        public WizBulbFactory()
        {
            bulb = new WizLightBulb();
            ServiceName = ServiceEnum.WizBulb;
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
                var mac = bulb.MacFinder(ip);

                if (mac.Equals(Utils.Common))
                    search = "None";

                return search switch
                {
                    "SYSTEMNAME" => Task.Run(() => new Result { Value = $"WizBulb_{mac}", FoundAt = "WizBulb" }),
                    "Serial" => Task.Run(() => new Result { Value = mac, FoundAt = "WizBulb" }),
                    _ => Task.Run(() => new Result { Value = Utils.Common, FoundAt = "N/A" }),
                };
            }
            catch (Exception)
            {
                return Task.Run(() => new Result { Value = Utils.Common, FoundAt = "N/A" });
            }
        }
    }
}