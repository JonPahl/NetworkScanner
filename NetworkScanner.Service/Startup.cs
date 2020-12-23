using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure;
using NetworkScanner.Infrastructure.Database;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetworkScanner.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public string[] Args { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var items = configuration.GetSection("args");
            var x = new List<string>();
            foreach(var item in items.GetChildren().ToList())
            {
                x.Add(item.Value);
            }
            Args = x.ToArray();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure your services here

            services.AddSingleton(LoggerFactory.Create(builder => builder.AddSerilog(dispose: true)));
            services.AddLogging();

            //var writer = new WrappingWriter(Console.Out);

            // Add access to generic IConfigurationRoot
            services.AddSingleton(Configuration);
            services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));

            //services.AddSingleton(writer);
            services.AddInfrastructure((IConfigurationRoot)Configuration);
            services.AddHostedService<NetworkScanner>();

            services.AddTransient(x => new NetworkScanner(x.GetRequiredService<NetworkContext>()));
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Hook in the global error-handling middleware
            //app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Register any middleware to report exceptions 
            //to a third-party service *after* our ErrorHandlingMiddleware

            //app.UseExcepticon();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseExceptionHandler();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
