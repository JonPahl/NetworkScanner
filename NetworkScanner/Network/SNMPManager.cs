
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using NetworkScanner.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace NetworkScanner.Network
{
    public class SNMPManager
    {
        public SNMPManager()
        {

        }


        public string FindValueInAllCommunitities(string ip, string oid)
        {
            var value = "N/A";
            foreach (var community in Utils.LoadCommunities())
            {
                try
                {
                    var result = Messenger.Get(VersionCode.V1,
                                   new IPEndPoint(IPAddress.Parse(ip), 161),
                                   new OctetString(community),
                                   new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                                   60000);

                    value = result.FirstOrDefault().Data.ToString();
                }
                catch (Exception ex)
                {
                    value = "N/A";
                }
            }

            return value;
        }


        /// <summary>
        /// Search a community for the specified oid and a value is found.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public string FindValue(string oid)
        {
            try
            {
                var result = Messenger.Get(VersionCode.V1,
                           new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161),
                           new OctetString("public"),
                           new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                           60000);

                return result.FirstOrDefault().Data.ToString();

                //var results = snmp.Get(SnmpVersion.Ver2, new string[] { oid });
                //return (results != null) ? results.FirstOrDefault().Value.ToString() : "";
            }
            catch (NullReferenceException)
            {
                return "N/A";
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

            //var results = snmp.Walk(SnmpVersion.Ver2, oid); //".1.3.6.1.2.1.1"
            //return (results?.Count > 0) ? results : null;
        }

    }
}

//using NetworkScanner.Extension;
//using SnmpSharpNet;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace NetworkScanner.Network
//{
//    /// <summary>
//    /// SNMP manager looks up and returns a single version.
//    /// </summary>
//    public class SNMPManager
//    {
//        protected SimpleSnmp snmp;

//        public SNMPManager()
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="SNMPManager"/> class.
//        /// </summary>
//        /// <param name="host">The host.</param>
//        /// <param name="community">The community.</param>
//        public SNMPManager(string host, string community)
//        {
//            snmp = new SimpleSnmp(host, community);
//        }

//        public string FindValueInAllCommunitities(string ip, string oid)
//        {
//            //"1.3.6.1.2.1.1.5.0"
//            var value = "N/A";
//            foreach (var community in Utils.LoadCommunities())
//            {
//                try
//                {
//                    snmp = new SimpleSnmp(ip, community);

//                    value = FindValue(oid);

//                    if (string.IsNullOrEmpty(value))
//                        value = "N/A";
//                }
//                catch (Exception)
//                {
//                    value = "N/A";
//                }
//            }
//            return value;
//        }

//        /// <summary>
//        /// Search a communitity for the specified oid and a value is found.
//        /// </summary>
//        /// <param name="oid">The oid.</param>
//        /// <returns></returns>
//        public string FindValue(string oid)
//        {
//            try
//            {
//                var results = snmp.Get(SnmpVersion.Ver2, new string[] { oid });
//                return (results != null) ? results.FirstOrDefault().Value.ToString() : "";
//            }
//            catch (NullReferenceException)
//            {
//                return "N/A";
//            }
//        }

//        /// <summary>
//        /// Loads the oid walk. Walks OID endpoint, the provided oid is the starting point.
//        /// </summary>
//        /// <param name="oid">The oid.</param>
//        /// <returns></returns>
//        public Dictionary<Oid, AsnType> LoadOidWalk(string oid)
//        {
//            var results = snmp.Walk(SnmpVersion.Ver2, oid); //".1.3.6.1.2.1.1"
//            return (results?.Count > 0) ? results : null;
//        }
//    }
//}