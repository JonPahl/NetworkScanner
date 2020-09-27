using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetworkScanner.Service
{
    public static class Program
    {
        /// <summary>
        /// Entry point into console application.
        /// </summary>
        private static async Task Main()
        {
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
                    services.AddSingleton(args[0] as TextWriter);
                    services.AddInfrastructure();
                    services.AddHostedService<NetworkScanner>();
                });
    }
}