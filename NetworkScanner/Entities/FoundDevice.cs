using System;
using System.Diagnostics.CodeAnalysis;

namespace NetworkScanner.Entities
{
    public class FoundDevice : IEquatable<FoundDevice>
    {
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string FoundUsing { get; set; }
        public DateTime FoundAt { get; set; }

        public bool Equals([AllowNull] FoundDevice other)
        {
            if (other == null)
                return false;

            if (other.DeviceId == null)
            {
                var result = (DeviceName == other.DeviceName && IpAddress == other.IpAddress);
                return !result;
            }
            else
            {
                var result = (DeviceId == other.DeviceId && DeviceName == other.DeviceName && IpAddress == other.IpAddress);
                return !result;
            }
        }
    }
}
