using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using System;
using System.Collections.Generic;

namespace NetworkScanner.Infrastructure.HostName
{
    /// <summary>
    /// Custom unit of work to find Reachable IP Addresses
    /// </summary>
    public class PingWorkflow : Auow
    {
        /// <summary>
        /// Lookup device names based on provided object.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public override Result FindDeviceName(FoundDevice device)
        {
            const string Property = "SYSTEMNAME";
            Factories = new List<Factory.ARpcFactory>
            {
                Factory.LoadFactory(ServiceEnum.DEVICENAME),
                Factory.LoadFactory(ServiceEnum.SNMP)
            };

            try
            {
                foreach (var f in Factories)
                {
                    var result = f.FindValue(device.IpAddress, Property).GetAwaiter().GetResult();

                    if (result.Value != Utils.Common)
                    {
                        Console.WriteLine($"{Property} :: {f.ServiceName} | {device.IpAddress} | {result.Value}");
                        return new Result { Value = result.Value, FoundAt = f.ServiceName.ToString() };
                    }
                }

                Console.WriteLine($"{Property} :: Factory Found no value. | {device.IpAddress} | {Utils.Common}");
                return new Result { Value = Utils.Common };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Result { Value = Utils.Common, FoundAt = Utils.Common };
            }
        }

        /// <summary>
        /// Build object to display results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ip"></param>
        /// <returns></returns>
        public override T BuildObject<T>(string ip)
        {
            var device = base.BuildObject<FoundDevice>(ip);
            device.Id = 0;
            device.FoundUsing = "PIPE";
            device.FoundAt = DateTime.Now;
            //device.Key = "";
            device.Key = device.GetHashCode();

            return (T)Convert.ChangeType(device, device.GetType());
        }

        /// <summary>
        /// Lookup Device ID
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public override Result FindDeviceId(FoundDevice device)
        {
            const string Property = "Serial";

            Factories = new List<Factory.ARpcFactory>()
            {
                Factory.LoadFactory(ServiceEnum.SSH),
                Factory.LoadFactory(ServiceEnum.WMI),
            };

            try
            {
                foreach (var factory in Factories)
                {
                    if (factory.ServiceName.Equals(ServiceEnum.SSH))
                        factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });

                    var result = factory.FindValue(device.IpAddress, Property)
                        .GetAwaiter().GetResult();
                    if (result.Value != Utils.Common)
                    {
                        Console.WriteLine($"{Property} :: {factory.ServiceName} | {device.IpAddress} | {result.Value}");
                        return new Result { Value = result.Value, FoundAt = factory.ServiceName.ToString() };
                    }
                }

                Console.WriteLine($"{Property} :: Factory Found no value. | {device.IpAddress} | {Utils.Common}");
                return new Result { Value = Utils.Common };
            }
            catch (Exception)
            {
                return new Result
                {
                    Value = Utils.Common,
                    FoundAt = Utils.Common
                };
            }
        }
    }
}
