/*
using NetworkScanner.Application.Common;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NetworkScanner.Application.WizBulb
{
    /// <summary>
    /// WizBulb lookup
    /// </summary>
    /// <seealso cref="AHandler"/>
    public class WizBulbHandler : AHandler
    {
        private readonly ARpcFactory factory;
        private readonly Func<ServiceEnum, ARpcFactory> resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizBulbHandler"/> class.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        public WizBulbHandler(Func<ServiceEnum, ARpcFactory> serviceResolver)
        {
            resolver = serviceResolver;
            this.factory = resolver(ServiceEnum.WizBulb);
            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        protected override Result Result { get; set; }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override async Task HandleRequest(object request)
        {
            var device = (FoundDevice)request;

            var Result = await factory.FindValue(device.IpAddress, "").ConfigureAwait(false);

            if (!Result.Equals(Utils.Common))
            {
                device.DeviceName = $"WizBulb_{Result.Value}";
                device.DeviceId = Result.Value;
                device.FoundUsing = "WizLightBulb";
            }

            SetupNext(device);
            _ = successor.HandleRequest(device);
        }

        /// <summary>
        /// Handles the request with result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public override Task<Result> HandleRequestWithResult(object request)
        {
            var device = (FoundDevice)request;

            var value = factory.FindValue(device.IpAddress, "");
            return Task.Run(() => value);

            //if (!value.Equals(Utils.Common)) {
            //    Result.FoundAt = "WizBulb";
            //    Result.Value = value;
            //    //Result.SetName($"WizBulb_{value}");
            //    //Result.SetDeviceId(value);
            //    return Task.Run(() => Result);
            //} else { return Task.Run(() => Result); }}

        protected override void SetupNext<T>(T device) { SetSuccessor(new PrintResultHandler()); }
    }
}
*/