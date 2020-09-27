using System;
using System.Collections.Generic;

namespace NetworkScanner.Domain.Entities
{
    public static class Utils
    {
        public static string Common = "N/A";

        public static List<string> LoadCommunities()
        {
            var communities = new List<string>
            {
                "Public",
                "public",
                "community"
            };

            return communities;
        }

        // public static List<string> LoadServices(){var svs = new List<string>{"_esp_temp._tcp.local"}; return svs;}

        public static List<ConnectionOptions> LoadSshConections(string ip)
        {
            var SshConnections = new List<ConnectionOptions>
            {
                new ConnectionOptions(ip, "pi", "password"),
                new ConnectionOptions(ip, "pi", "raspberry")
            };

            return SshConnections;
        }

        public static List<string> LoadMdnsServices()
        {
            return new List<string>()
            {
                 "_esp_temp._tcp.local",
                 "_esp_Hmd._tcp.local"
            };
        }

        public static List<KeyValuePair<string, string>> LoadSNMPOptions()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("SYSTEMNAME", "1.3.6.1.2.1.1.5.0")
            };
        }
    }
}