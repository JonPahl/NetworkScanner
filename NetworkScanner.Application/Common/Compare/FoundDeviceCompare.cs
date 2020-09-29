using NetworkScanner.Domain.Entities;
using System.Collections.Generic;

namespace NetworkScanner.Application.Compare
{
    /// <summary>
    /// Compare two IpAddresses
    /// </summary>
    /// <seealso cref="IComparer{FoundDevice}"/>
    public class FoundDeviceCompare : IComparer<FoundDevice>
    {
        public int Compare(FoundDevice item1, FoundDevice item2)
        {
            var x = item1.IpAddress;
            var y = item2.IpAddress;

            string ip1 = x + '.', ip2 = y + '.';
            string xSection = "", ySection = "";
            for (int i = 0; i < ip1.Length && i < ip2.Length; i++)
            {
                if (ip1[i] == '.' && ip2[i] == '.')
                {
                    if (xSection != ySection)
                        return int.Parse(xSection) - int.Parse(ySection);
                    xSection = ""; // Start compare the next section
                    ySection = "";
                }
                else if (ip1[i] == '.') return -1; //The first section is smaller because it's length is smaller
                else if (ip2[i] == '.') return 1;
                else
                {
                    xSection += ip1[i];
                    ySection += ip2[i];
                }
            }
            return 0;
        }

        public static class ExtensionMethods
        {
            /// <summary>
            /// If we would find any difference between any section it would already return
            /// something. so that mean that both IPs are the same.
            /// </summary>
            /// <param name="item1">The item1.</param>
            /// <param name="item2">The item2.</param>
            /// <returns></returns>
            public static int FoundDeviceExtension(FoundDevice item1, FoundDevice item2)
            {
                var x = item1.IpAddress;
                var y = item2.IpAddress;

                string ip1 = x + '.', ip2 = y + '.';
                string xSection = "", ySection = "";
                for (int i = 0; i < ip1.Length && i < ip2.Length; i++)
                {
                    if (ip1[i] == '.' && ip2[i] == '.')
                    {
                        if (xSection != ySection)
                            return int.Parse(xSection) - int.Parse(ySection);
                        xSection = ""; // Start compare the next section
                        ySection = "";
                    }
                    else if (ip1[i] == '.') return -1; //The first section is smaller because it's length is smaller
                    else if (ip2[i] == '.') return 1;
                    else
                    {
                        xSection += ip1[i];
                        ySection += ip2[i];
                    }
                }
                return 0;
                //If we would find any difference between any section it would already return something.
                //so that mean that both IPs are the same
            }

            /// <summary>
            /// If we would find any difference between any section it would already return
            /// something. so that mean that both IPs are the same
            /// </summary>
            /// <param name="item1">The item1.</param>
            /// <param name="item2">The item2.</param>
            /// <returns></returns>
            public static int KeyValueIpCompare(KeyValuePair<string, FoundDevice> item1, KeyValuePair<string, FoundDevice> item2)
            {
                var first = item1.Value.IpAddress;
                var second = item2.Value.IpAddress;

                string ip1 = first + '.', ip2 = second + '.';
                string xSection = "", ySection = "";
                for (int i = 0; i < ip1.Length && i < ip2.Length; i++)
                {
                    if (ip1[i] == '.' && ip2[i] == '.')
                    {
                        if (xSection != ySection)
                            return int.Parse(xSection) - int.Parse(ySection);
                        xSection = ""; // Start compare the next section
                        ySection = "";
                    }
                    else if (ip1[i] == '.') return -1; //The first section is smaller because it's length is smaller
                    else if (ip2[i] == '.') return 1;
                    else
                    {
                        xSection += ip1[i];
                        ySection += ip2[i];
                    }
                }
                return 0;
            }
        }
    }
}