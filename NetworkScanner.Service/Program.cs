using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure;
using NetworkScanner.Infrastructure.Database;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetworkScanner.Service
{
    public static class Program
    {
        public static IConfigurationRoot configuration;

        /// <summary>
        /// Entry point into console application.
        /// </summary>
        private static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
     .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
     .MinimumLevel.Debug()
     .Enrich.FromLogContext()
     .CreateLogger();

            try
            {
                #region Setup Services

                var writer = new WrappingWriter(Console.Out);

                object[] args = new object[] { writer };

                var host = CreateHostBuilder(args).Build();
                using (host)
                    await host.RunAsync().ConfigureAwait(false);

                #endregion Setup Services
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(object[] args) =>
            Host.CreateDefaultBuilder(new string[] { })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(LoggerFactory.Create(builder => builder.AddSerilog(dispose: true)));
                    services.AddLogging();
                    // Build configuration
                    configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile("appsettings.json", false)
                        .Build();
                    // Add access to generic IConfigurationRoot
                    services.AddSingleton(configuration);
                    services.Configure<LiteDbOptions>(configuration.GetSection("LiteDbOptions"));

                    services.AddSingleton(args[0] as TextWriter);
                    services.AddInfrastructure(configuration);
                    services.AddHostedService<NetworkScanner>();

                    services.AddTransient(x => new NetworkScanner(x.GetRequiredService<NetworkContext>()));
                });
    }
}