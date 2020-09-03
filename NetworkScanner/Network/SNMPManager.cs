using NetworkScanner.Extension;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Network
{
    /// <summary>
    /// SNMP manager looks up and returns a single version.
    /// </summary>
    public class SNMPManager
    {
        protected SimpleSnmp snmp;

        public SNMPManager()
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SNMPManager"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="community">The community.</param>
        public SNMPManager(string host, string community)
        {
            snmp = new SimpleSnmp(host, community);
        }

        public string FindValueInAllCommunitities(string ip, string oid)
        {
            //"1.3.6.1.2.1.1.5.0"
            var value = "N/A";
            foreach (var community in Utils.LoadCommunities())
            {
                try
                {
                    snmp = new SimpleSnmp(ip, community);

                    value = FindValue(oid);

                    if (string.IsNullOrEmpty(value))
                    {
                        value = "N/A";
                    }
                    //else{break;}
                }
                catch (Exception ex)
                {
                    var x = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public string FindValue(string oid)
        {
            var results = snmp.Get(SnmpVersion.Ver2, new string[] { oid });
            return (results != null) ? results.FirstOrDefault().Value.ToString() : "";
        }

        /// <summary>
        /// Loads the oid walk. Walks OID endpoint, the provided oid is the starting point.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public Dictionary<Oid, AsnType> LoadOidWalk(string oid)
        {
            var results = snmp.Walk(SnmpVersion.Ver2, oid); //".1.3.6.1.2.1.1"
            return (results?.Count > 0) ? results : null;
        }
    }
}