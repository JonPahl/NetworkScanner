using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetworkScanner.Infrastructure.SNMP
{
    /// <summary>
    /// SNMP Manager
    /// </summary>
    public class SNMPManager
    {
        public string FindValueInAllCommunitities(string ip, string oid)
        {
            var value = Utils.Common;
            foreach (var community in Utils.LoadCommunities())
            {
                try
                {
                    var result = Messenger.Get(
                        VersionCode.V2,
                        new IPEndPoint(IPAddress.Parse(ip), 161),
                        new OctetString(community),
                        new List<Variable> {
                            new Variable(new ObjectIdentifier(oid)) },
                        6000);

                    value = result.FirstOrDefault()?.Data.ToString();
                    if (!string.IsNullOrEmpty(value) && !value.Equals(Utils.Common))
                    {
                        var args = new RcdFndEventArgs { Value = value, IpAddress = ip };
                        OnValueFound(args);
                        break;
                    }
                }
                catch (Exception)
                {
                    value = Utils.Common;
                }
            }

            OnValueFound(new RcdFndEventArgs { Value = value, IpAddress = ip });

            return value;
        }

        /// <summary>
        /// Search a community for the specified OID and a value is found.
        /// </summary>
        /// <param name="oid">The OID.</param>
        /// <returns></returns>
        public string FindValue(string oid, string ipAddress)
        {
            try
            {
                var result = Messenger.Get(VersionCode.V2,
                           new IPEndPoint(IPAddress.Parse(ipAddress), 161),
                           new OctetString("public"),
                           new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                           6000);

                return result.FirstOrDefault()?.Data.ToString();
            }
            catch (NullReferenceException)
            {
                return Utils.Common;
            }
        }

        /// <summary>
        /// Loads the OID walk. Walks OID endpoint, the provided OID is the starting point.
        /// </summary>
        /// <param name="oid">Starting OID </param>
        /// <param name="ip">IP address</param>
        /// <param name="community"> OID Community </param>
        /// <returns> <list type="Variable"/> </returns>
        public List<Variable> LoadOidWalk(string oid,
            string ip, string community = "public")
        {
            var result = new List<Variable>();

            Messenger.Walk(VersionCode.V1,
                           new IPEndPoint(IPAddress.Parse(ip), 161),
                           new OctetString(community),
                           new ObjectIdentifier(oid),
                           result,
                           60000,
                           WalkMode.WithinSubtree);

            return result;
        }

        protected virtual void OnValueFound(RcdFndEventArgs e) => ValueFound?.Invoke(this, e);

        public event EventHandler<RcdFndEventArgs> ValueFound;
    }

    public class RcdFndEventArgs
    {
        public DateTime TimeReached { get; set; }
        public string Value { get; set; }
        public string IpAddress { get; set; }
    }
}