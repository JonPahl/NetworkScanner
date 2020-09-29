using Microsoft.Extensions.DependencyInjection;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain;
using NetworkScanner.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Infrastructure
{
    public static class DependencyInjection
    {
        public delegate ARpcFactory RpcFactoryResolver(string key);

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<DeviceNameFactory>();
            //services.AddScoped<MdnsFactory>();
            services.AddScoped<SnmpFactory>();
            services.AddScoped<WmiFactory>();
            services.AddScoped<SSHFactory>();
            services.AddScoped<WizBulbFactory>();

            services.AddScoped<IPropertyLookup, PropertyLookup>();

            services.AddTransient<Func<ServiceEnum, ARpcFactory>>(_ => Key =>
            {
                var factories = typeof(ARpcFactory)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ARpcFactory)) && !t.IsAbstract)
                .Select(t => (ARpcFactory)Activator.CreateInstance(t)).ToList();

                return factories.Find(x => x.ServiceName.Equals(Key));
            });

            services.AddTransient(_ =>
            {
                var factories = new List<ARpcFactory>();
                var values = Enum.GetValues(typeof(ServiceEnum)).Cast<ServiceEnum>();

                var fs = typeof(ARpcFactory)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(ARpcFactory)) && !t.IsAbstract)
                    .Select(t => (ARpcFactory)Activator.CreateInstance(t)).ToList();

                foreach (var service in values)
                {
                }

                //var x = typeof(ARpcFactory)
                //.Assembly.GetTypes()
                //.Where(t => t.IsSubclassOf(typeof(ARpcFactory)) && !t.IsAbstract)
                //.Select(t => (ARpcFactory)Activator.CreateInstance(t)).ToList();

                return fs;
            });

            /*
            services.AddTransient<Func<ServiceEnum, ARpcFactory>>(serviceProvider => key => {
                return key switch
                {
                    ServiceEnum.WMI => serviceProvider.GetService<SnmpFactory>(),
                    ServiceEnum.SNMP => serviceProvider.GetService<SnmpFactory>(),
                    ServiceEnum.SSH => serviceProvider.GetService<SshFactory>(),
                    ServiceEnum.WizBulb => serviceProvider.GetService<WizBulbFactory>(),
                    ServiceEnum.DEVICENAME => throw new NotImplementedException(),
                    ServiceEnum.MDNS => throw new NotImplementedException(),
                    _ => null,
                };
            });
            */

            return services;
        }
    }
}
