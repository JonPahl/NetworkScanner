/*
using NetworkScanner.Application.Common;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NetworkScanner.Application.WMI
{
    /// <summary>
    /// Connect to remote system and for details using WMI protocol.
    /// </summary>
    /// <seealso cref="AHandler"/>
    public class WmiHostNameHandler : AHandler
    {
        private readonly ARpcFactory factory;
        private readonly Func<ServiceEnum, ARpcFactory> resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiHostNameHandler"/> class.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        public WmiHostNameHandler(Func<ServiceEnum, ARpcFactory> serviceResolver)
        {
            resolver = serviceResolver;
            this.factory = resolver(ServiceEnum.WMI);

            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        protected override Result Result { get; set; }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override async Task HandleRequest(object request)
        {
            //Task.CompletedTask;
        }

        /*public override async Task HandleRequest(object request)
        {
            var device = (FoundDevice)request;

            var value = factory.FindValue(device.IpAddress, "SYSTEMNAME");

            if (!value.Equals(Utils.Common))
            {
                return value;
            } else
            {
                return Utils.Common;
            }

            //if (!value.Equals(Utils.Common))
            //{
            //    device.DeviceName = value;
            //    device.FoundUsing = "WMI";
            //}

            //SetupNext(device);
            //return successor.HandleRequest(device);
        }*

        public override async Task<Result> HandleRequestWithResult(object request) {
            var device = (FoundDevice)request;var result = await factory.FindValue(device.IpAddress, "SYSTEMNAME");
            if (!result.Equals(Utils.Common)) { Result.FoundAt = "WMI"; Result.Value = result.Value; return Result;
            } else { return Result; }} 

        /* private void SetupNext(FoundDevice device) {
             if (!device.DeviceName.Equals(Utils.Common)) {
                 SetSuccessor(new PrintResultHandler());
             } else {
                 //var factory = resolver(ServiceEnum.SNMP);
                 SetSuccessor(new WizBulbHandler(resolver));
                 var factory = resolver(ServiceEnum.SSH);
                 factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });

                 SetSuccessor(new SshLookupHandler(factory));
                 successor.SetSuccessor(new PrintResultHandler());
             }}
    }
}
*/