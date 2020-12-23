using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetworkScanner.Domain.Entities;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetworkScanner.Service
{
    public static class Program
    {
        public static async Task Main()
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

                //var host = CreateHostBuilder(args).Build();
                //await host.RunAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(object[] args) =>
          Host.CreateDefaultBuilder(Array.Empty<string>())
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureServices(services => services.AddSingleton(args[0] as TextWriter));

    /*
    public static IHostBuilder CreateHostBuilder(object[] args)=>Host.CreateDefaultBuilder(new string[]{})
    ConfigureServices(services =>{})
       //.ConfigureAppConfiguration((hostingContext,config)=>config.AddCommandLine(args))
       .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());*/

}
}