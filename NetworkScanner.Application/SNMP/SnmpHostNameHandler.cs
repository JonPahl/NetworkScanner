/*
using NetworkScanner.Application.Common;
using NetworkScanner.Application.WMI;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NetworkScanner.Application.SNMP {
    /// <summary>
    /// </summary>
    /// <seealso cref="AHandler"/>
    public class SnmpHostNameHandler : AHandler {
        private readonly ARpcFactory factory;
        private readonly Func<ServiceEnum, ARpcFactory> resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpHostNameHandler"/> class.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        public SnmpHostNameHandler(Func<ServiceEnum, ARpcFactory> serviceResolver) {
            resolver = serviceResolver;
            this.factory = resolver(ServiceEnum.SNMP);
            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        protected override Result Result { get; set; }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override async Task HandleRequest(object request)
        {
            // device = (FoundDevice)request;
            //var value = factory.FindValue(device.IpAddress, "SYSTEMNAME");

            //SetupNext(device);
            //return successor.HandleRequest(device);
        }

        /// <summary>
        /// Handles the request with result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public override async Task<Result> HandleRequestWithResult(object request) {
            try {
                var device = (FoundDevice)request;
                var Result = factory.FindValue(device.IpAddress, "SYSTEMNAME").ConfigureAwait(false);                
                return Task.Run(() => Result);
            } catch (TimeoutException ex) {
                Result = new Result { Value = Utils.Common, FoundAt = "" };
                return Task.Run(() => Result);
            } catch(Exception ex) {
                Result = new Result { Value = Utils.Common, FoundAt = "" };
                return Task.Run(() => Result);
            }
        }

        protected void SetupNext(FoundDevice device) {
            if (!device.DeviceName.Equals(Utils.Common)) { SetSuccessor(new PrintResultHandler()); }else{
                //var factory = resolver(ServiceEnum.SNMP);
                SetSuccessor(new WmiHostNameHandler(resolver));

                /*
                var factory = resolver(ServiceEnum.SSH);
                factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });

                SetSuccessor(new SshLookupHandler(factory));
                successor.SetSuccessor(new PrintResultHandler());
            }}}} */