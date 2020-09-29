/*
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using NetworkScanner.Application.Common;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;

namespace NetworkScanner.Application.DnsHostname
{
    /// <summary>
    /// Ping IpAddress
    /// </summary>
    /// <seealso cref="Handler"/>
    public class PingHandler : AHandler
    {
        private readonly Func<ServiceEnum, ARpcFactory> resolver;

        public PingHandler(Func<ServiceEnum, ARpcFactory> resolver)
        {
            this.resolver = resolver;
            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        protected override Result Result { get; set; }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override async Task HandleRequest(object request)
        {
            var ping = new Ping();
            var pingResult = ping.Send(request.ToString());

            if (pingResult.Status == IPStatus.Success)
            {
                Console.WriteLine("Host {0} is reachable", request.ToString());

                var device = new FoundDevice
                {
                    IpAddress = pingResult.Address.ToString(),
                    DeviceName = Utils.Common,
                    DeviceId = Utils.Common,
                    FoundAt = DateTime.Now,
                    FoundUsing = "Pipe"
                };

                successor?.HandleRequest(device);
            }
        }
    }
}
*/