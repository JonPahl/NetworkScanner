using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace NetworkScanner.Infrastructure.SNMP
{
    public class SNMPManager
    {
        public SNMPManager()
        {
        }

        public string FindValueInAllCommunitities(string ip, string oid)
        {
            var value = Utils.Common;
            foreach (var community in Utils.LoadCommunities())
            {
                try
                {
                    var result = Messenger.Get(VersionCode.V2,
                                   new IPEndPoint(IPAddress.Parse(ip), 161),
                                   new OctetString(community),
                                   new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                                   6000);

                    value = result.FirstOrDefault()?.Data.ToString();
                    if (!string.IsNullOrEmpty(value) && !value.Equals(Utils.Common))
                    {
                        var args = new RecordFoundEventArgs { Value = value, IpAddress = ip };
                        OnValueFound(args);

                        break;
                    }
                }
                catch (Exception)
                {
                    value = Utils.Common;
                }
            }

            OnValueFound(new RecordFoundEventArgs { Value = value, IpAddress = ip });

            return value;
        }

        /// <summary>
        /// Search a community for the specified oid and a value is found.
        /// </summary>
        /// <param name="oid">The oid.</param>
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

                //var results = snmp.Get(SnmpVersion.Ver2, new string[] { oid });
                //return (results != null) ? results.FirstOrDefault().Value.ToString() : "";
            }
            catch (NullReferenceException)
            {
                return Utils.Common;
            }
        }

        /// <summary>
        /// Loads the oid walk. Walks OID endpoint, the provided oid is the starting point.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public List<Variable> LoadOidWalk(string oid)
        {
            var result = new List<Variable>();

            Messenger.Walk(VersionCode.V1,
                           new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161),
                           new OctetString("public"),
                           new ObjectIdentifier(oid),
                           result,
                           60000,
                           WalkMode.WithinSubtree);

            return result;
        }

        protected virtual void OnValueFound(RecordFoundEventArgs e)
        {
            ValueFound?.Invoke(this, e);
        }

        public event EventHandler<RecordFoundEventArgs> ValueFound;

        public class RecordFoundEventArgs //: EventArgs
        {
            public DateTime TimeReached { get; set; }
            public string Value { get; set; }
            public string IpAddress { get; set; }
        }

    }
}