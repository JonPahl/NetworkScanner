using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace NetworkScanner.Application.Compare
{
    /// <summary>
    /// Compares two IP addresses.
    /// http://stackoverflow.com/questions/4785218/linq-lambda-orderby-delegate-for-liststring-of-ip-addresses
    /// </summary>
    public class IpComparer : IComparer<IPAddress>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="first"/> and <paramref name="second"/>, as shown in the following table.
        /// Value Meaning Less than zero<paramref name="first"/> is less than <paramref name="second"/>.
        /// Zero<paramref name="first"/> equals <paramref name="second"/>.
        /// Greater than zero <paramref name="first"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="first">The first object to compare.</param><param name="second">The second object to compare.</param>
        public int Compare(IPAddress first, IPAddress second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            byte[] bytesOfX = first.GetAddressBytes();
            byte[] bytesOfY = second.GetAddressBytes();

            return StructuralComparisons.StructuralComparer.Compare(bytesOfX, bytesOfY);
        }
    }
}