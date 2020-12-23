using NetworkScanner.Domain.Entities;
using System.Collections.Generic;

namespace NetworkScanner.Application.Compare
{
    /// <summary>
    /// Compare two IP Addresses
    /// </summary>
    /// <seealso cref="IComparer{FoundDevice}"/>
    public class FoundDeviceCompare : IComparer<FoundDevice>
    {
        public int Compare(FoundDevice item1, FoundDevice item2)
        {
            var ipOne = item1.IpAddress;
            var ipTwo = item2.IpAddress;

            string ip1 = ipOne + '.', ip2 = ipTwo + '.';
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
                else if (ip1[i] == '.')
                {
                    return -1;
                }
                // The first section is smaller because it's length is smaller
                else if (ip2[i] == '.')
                {
                    return 1;
                }
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