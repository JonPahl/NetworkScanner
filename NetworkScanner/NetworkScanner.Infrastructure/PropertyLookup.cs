using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure
{
    public class PropertyLookup : IPropertyLookup
    {
        protected ARpcFactory Factory;
        protected List<ARpcFactory> Factories;

        public PropertyLookup()
        {
            Factory = new RpcFactory();
            Factories = new List<ARpcFactory>();

            Factories = Factory.LoadFactories();

            var temp01 = Factories[0];
            var temp02 = Factories[1];
            Factories.Clear();
            Factories.Add(temp01);
            Factories.Add(temp02);
        }

        public Result FindValue(FoundDevice device, string Property)
        {
            try
            {
                List<Task<Result>> tasks = new List<Task<Result>>();
                foreach (var factory in Factories)
                {
                    try
                    {
                        #region Switch cases
                        switch (factory.ServiceName)
                        {
                            case ServiceEnum.DEVICENAME:
                                break;
                            case ServiceEnum.WMI:
                                break;
                            case ServiceEnum.SNMP:
                                break;
                            case ServiceEnum.SSH:
                                factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });
                                break;
                            case ServiceEnum.WizBulb:
                                break;
                        }
                        #endregion
                        tasks.Add(factory.FindValue(device.IpAddress, Property));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                Task.WaitAll(tasks.ToArray());
                var deviceResults = Task.WhenAll(tasks.ToList()).GetAwaiter().GetResult().Where(x => x.Value != null);
                var found = deviceResults.Where(x => !x.Value.Equals(Utils.Common)).ToList();
                return (found.Count.Equals(0)) ? new Result() { Value = Utils.Common } : found.FirstOrDefault();
            }
            catch (Exception)
            {
                return new Result() { Value = Utils.Common };
            }
        }
    }
}
