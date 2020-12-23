using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using Rssdp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipleLineExample.Ssdp
{
    /// <summary>
    /// RSDP Unit of Work
    /// </summary>
    public class RssdpUow : Auow
    {
        public override TOutput BuildObject<TOutput, TInput>(TInput raw)
        {
            try
            {
                var objs = raw as List<object>;
                var device = objs.FirstOrDefault() as DiscoveredSsdpDevice;
                objs.RemoveAt(0);
                var info = objs.FirstOrDefault() as SsdpDevice;

                var Uuid = info.CustomProperties?.Count > 0 ?
                    info.CustomProperties.FirstOrDefault(x => x.Name.Equals("X_hardwareId") || x.Name.Equals("deviceID"))?.Value : info.Uuid;

                var foundDevice = new FoundDevice
                {
                    IpAddress = device.DescriptionLocation.DnsSafeHost,
                    DeviceName = info.FriendlyName,
                    DeviceId = Uuid.Replace("-", "") ?? Utils.Common,
                    FoundUsing = "SSdp",
                    FoundAt = device.AsAt.LocalDateTime
                };

                return (TOutput)Convert.ChangeType(foundDevice, foundDevice.GetType());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}