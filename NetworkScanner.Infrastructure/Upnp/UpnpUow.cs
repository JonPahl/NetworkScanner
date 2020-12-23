using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using Rssdp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Infrastructure.Upnp
{
    /// <summary>
    /// Unit of work for UPNP Protocol
    /// </summary>
    public class UpnpUow : Auow
    {
        public override TOutput BuildObject<TOutput, TInput>(TInput raw)
        {
            var itm = raw as List<object>;
            var device = itm.FirstOrDefault() as DiscoveredSsdpDevice;
            var info = itm.LastOrDefault() as SsdpDevice;

            var foundDevice = new FoundDevice
            {
                DeviceId = info.Uuid.Replace("-", ""),
                DeviceName = info.FriendlyName,
                FoundAt = device.AsAt.LocalDateTime,
                IpAddress = device.DescriptionLocation.DnsSafeHost,
                FoundUsing = "UPNP"
            };

            return (TOutput)Convert.ChangeType(foundDevice, foundDevice.GetType());
        }
    }
}
