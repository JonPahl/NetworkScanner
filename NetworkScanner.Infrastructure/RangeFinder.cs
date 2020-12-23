﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

/* ====================================================================================
                C# IP address range finder helper class (C) Nahum Bazes
* Free for private & commercial use - no restriction applied, please leave credits.
*                              DO NOT REMOVE THIS COMMENT
* ==================================================================================== */
namespace NetworkScanner.Infrastructure
{
    public class RangeFinder
    {
        /// <summary>
        /// Build out a range of IP addresses between starting and ending address
        /// </summary>
        /// <param name="startIP">The start IP Address.</param>
        /// <param name="endIP">The end IP Address.</param>
        /// <returns>List of strings</returns>
        public IEnumerable<string> GetIPRange(IPAddress startIP, IPAddress endIP)
        {
            uint sIP = IpToUint(startIP.GetAddressBytes());
            uint eIP = IpToUint(endIP.GetAddressBytes());
            while (sIP <= eIP)
            {
                yield return new IPAddress(ReverseBytesArray(sIP)).ToString();
                sIP++;
            }
        }

        /// <summary>
        /// Reverses the bytes array. Reverse byte order in array
        /// </summary>
        /// <param name="ip">The IP Address</param>
        /// <returns>return Uint</returns>
        protected uint ReverseBytesArray(uint ip)
        {
            byte[] bytes = BitConverter.GetBytes(ip);
            bytes = bytes.Reverse().ToArray();
            return (uint)BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Convert IP byte array to uint.
        /// Convert bytes array to 32 bit long value
        /// </summary>
        /// <param name="ipBytes">The IP bytes.</param>
        /// <returns>return Uint</returns>
        protected uint IpToUint(byte[] ipBytes)
        {
            ByteConverter bConvert = new ByteConverter();
            uint ipUint = 0;

            int shift = 24; // indicates number of bits left for shifting
            foreach (byte b in ipBytes)
            {
                if (ipUint == 0)
                {
                    ipUint = (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint)) << shift;
                else
                    ipUint += (uint)bConvert.ConvertTo(b, typeof(uint));

                shift -= 8;
            }
            return ipUint;
        }
    }
}