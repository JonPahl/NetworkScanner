using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Workflow;
using System;
using System.Collections.Generic;

namespace NetworkScanner.Infrastructure.HostName
{
    public class PingWorkflow : Auow
    {
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

        public override T BuildObject<T>(string ip)
        {
            var fd = base.BuildObject<FoundDevice>(ip);
            fd.Id = "0";
            fd.FoundUsing = "PIPE";
            fd.FoundAt = DateTime.Now;
            fd.Key = "";

            fd.Key = fd.GetHashCode();

            return (T)Convert.ChangeType(fd, fd.GetType());
        }

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
                foreach (var f in Factories)
                {
                    if (f.ServiceName.Equals(ServiceEnum.SSH))
                        f.SetCommands(new List<object> { "cat /proc/cpuinfo" });

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
