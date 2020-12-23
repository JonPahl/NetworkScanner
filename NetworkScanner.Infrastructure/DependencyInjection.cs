using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain;
using NetworkScanner.Infrastructure.Database;
using NetworkScanner.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Infrastructure
{
    public static class DependencyInjection
    {
        public delegate ARpcFactory RpcFactoryResolver(string key);

        public static IConfigurationRoot Configuration;

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot configuration)
        {
            Configuration = configuration;
            services.AddScoped<DeviceNameFactory>();
            // services.AddScoped<MdnsFactory>();
            services.AddScoped<SnmpFactory>();
            services.AddScoped<WmiFactory>();
            services.AddScoped<SSHFactory>();
            services.AddScoped<WizBulbFactory>();

            services.AddScoped<IPropertyLookup, PropertyLookup>();

            services.AddSingleton<ILiteDbContext, NetworkContext>();
            services.AddTransient<NetworkContext>();

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

                return typeof(ARpcFactory)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(ARpcFactory)) && !t.IsAbstract)
                    .Select(t => (ARpcFactory)Activator.CreateInstance(t)).ToList();
            });

            return services;
        }
    }
}
