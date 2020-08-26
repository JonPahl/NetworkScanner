using SnmpSharpNet;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SNMPManager"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="community">The community.</param>
        public SNMPManager(string host, string community)
        {
            snmp = new SimpleSnmp(host, community);
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public string FindValue(string oid)
        {
            var results = snmp.Get(SnmpVersion.Ver1, new string[] { oid });
            return (results != null) ? results.FirstOrDefault().Value.ToString() : "";
        }

        /// <summary>
        /// Loads the oid walk. Walks OID endpoint, the provided oid is the starting point.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public Dictionary<Oid, AsnType> LoadOidWalk(string oid)
        {
            var results = snmp.Walk(SnmpVersion.Ver1, oid); //".1.3.6.1.2.1.1"
            return (results?.Count > 0) ? results : null;
        }

    }
}

// string host = "localhost";
//string community = "community";

//var results = snmp.Walk(SnmpVersion.Ver1, ".1.3.6.1.2.1.1.1");
//var results = LoadOidWalk(".1.3.6.1.2.1.1");

//if (results != null)
//{
//    foreach (var kvp in results)
//    {
//        Console.WriteLine("\t{0}: {1} {2}", kvp.Key.ToString(),
//                          SnmpConstants.GetTypeName(kvp.Value.Type),kvp.Value.ToString());
//    }}

//    snmp.Walk(SnmpVersion.Ver1, ".1.3.6.1.2.1.1");
//if (results?.Count > 0)
//{
//    foreach (var kvp in results)
//    {
//        Console.WriteLine("\t{0}: {1} {2}", kvp.Key.ToString(),
//                          SnmpConstants.GetTypeName(kvp.Value.Type),kvp.Value.ToString());
//    }
//}
//else
//{
//    Console.WriteLine("SNMP agent host name/ip address is invalid.");
//    return;
//}

/*
if (!snmp.Valid)
{
    Console.WriteLine("SNMP agent host name/ip address is invalid.");
    return;
}
Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver1, new string[] { ".1.3.6.1.2.1.1.1.0" });
if (result == null)
{
    Console.WriteLine("No results received.");
    return;
}

foreach (KeyValuePair<Oid, AsnType> kvp in result)
{
    Console.WriteLine("\t{0}: {1} {2}", kvp.Key.ToString(),
                          SnmpConstants.GetTypeName(kvp.Value.Type),
                          kvp.Value.ToString());
}
*/

//        Snmp snmp = null;
//        String address = null;

//        /**
//        * Constructor
//        * @param add
//        */
//        public SNMPManager(String add)
//        {
//            address = add;
//        }

//        public static void main(String[] args) throws IOException
//        {
//            /**
//            * Port 161 is used for Read and Other operations
//            * Port 162 is used for the trap generation
//            */
//            SNMPManager client = new SNMPManager("udp:127.0.0.1/161");
//        //System.out.println(client);
//        client.start();
//        /**
//        * OID - .1.3.6.1.2.1.1.1.0 => SysDec
//        * OID - .1.3.6.1.2.1.1.5.0 => SysName
//        * => MIB explorer will be useful here, as discussed in previous article
//        */
//        String sysDescr = client.getAsString(new OID("1.3.6.1.2.1.1.1.0"));
//        System.out.println(sysDescr);
//    }

//    /**
//    * Start the Snmp session. If you forget the listen() method you will not
//    * get any answers because the communication is asynchronous
//    * and the listen() method listens for answers.
//    * @throws IOException
//    */
//    void start() throws IOException
//    {
//        TransportMapping transport = new DefaultUdpTransportMapping();
//    snmp = new Snmp(transport);
//    // Do not forget this line!
//    transport.listen();
//    }

///**
//* Method which takes a single OID and returns the response from the agent as a String.
//* @param oid
//* @return
//* @throws IOException
//*/
//public String getAsString(OID oid) throws IOException
//{
//    ResponseEvent event = get(new OID[] { oid });
//    //System.out.println(oid);
//    return event.getResponse().get(0).getVariable().toString();
//    }

//    /**
//    * This method is capable of handling multiple OIDs
//    * @param oids
//    * @return
//    * @throws IOException
//    */
//    public ResponseEvent get(OID oids[]) throws IOException
//{
//    PDU pdu = new PDU();
//for (OID oid : oids)
//{
//    pdu.add(new VariableBinding(oid));

//}
//pdu.setType(PDU.GET);
//ResponseEvent event = snmp.send(pdu, getTarget(), null);
//        if(event != null) 
//        {
//    return event;
//}
//        throw new RuntimeException("GET timed out");
//    }

//    /**
//    * This method returns a Target, which contains information about
//    * where the data should be fetched and how.
//    * @return
//    */
//    private Target getTarget(){
//    Address targetAddress = GenericAddress.parse(address);
//    CommunityTarget target = new CommunityTarget();
//    target.setCommunity(new OctetString("public"));
//    target.setAddress(targetAddress);
//    target.setRetries(2);
//    target.setTimeout(1500);
//    target.setVersion(SnmpConstants.version2c);
//    return target;
//}}}
