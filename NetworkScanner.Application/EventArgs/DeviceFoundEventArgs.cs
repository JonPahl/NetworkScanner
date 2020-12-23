using System;
using NetworkScanner.Domain.Entities;

namespace NetworkScanner.Application.EventArgs
{
    /// <summary>
    /// Event fired when an IP address is reachable via Ping command.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class DeviceFoundEventArgs
    {
        public int Found { get; set; }
        public DateTime TimeReached { get; set; }
        public FoundDevice Item { get; set; }
    }
}
