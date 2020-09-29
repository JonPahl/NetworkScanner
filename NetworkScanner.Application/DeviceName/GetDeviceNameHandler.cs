/*
using NetworkScanner.Application.Common;
using NetworkScanner.Application.SNMP;
using NetworkScanner.Application.WizBulb;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkScanner.Application.DnsHostname {
    /// <summary>
    /// Check for DNS HostName.
    /// </summary>
    /// <seealso cref="Handler" />
    public class GetDeviceNameHandler : AHandler {
        private readonly Func<ServiceEnum, ARpcFactory> resolver;

        #region Move up
        protected SnmpHostNameHandler smp;
        protected WizBulbHandler wizBulb;
        protected GetHostEntryHandler HostEntry;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDeviceNameHandler"/> class.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        public GetDeviceNameHandler(Func<ServiceEnum, ARpcFactory> serviceResolver) {
            resolver = serviceResolver;
            smp = new SnmpHostNameHandler(resolver);
            wizBulb = new WizBulbHandler(resolver);
            HostEntry = new GetHostEntryHandler(resolver);
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

            try
            {
                var tasks = new List<Task<Result>>
                {
                    HostEntry.HandleRequestWithResult(device),
                    smp.HandleRequestWithResult(device),
                    wizBulb.HandleRequestWithResult(device),
                };

                Task.WaitAll(tasks.ToArray());
                var taskResults = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
               
                var found = taskResults.Where(x => !x.Value.Equals(Utils.Common)).ToList();

                if (found.Count.Equals(0))
                {
                    device.DeviceName = Utils.Common;
                }
                else
                {
                    var item = found.FirstOrDefault();
                    device.DeviceName = item.Value;
                    device.FoundUsing = item.FoundAt;

                    /*
                    if (item.GetDeviceId() != null) device.DeviceId = item.GetDeviceId();
                    if (item.GetName() != null) device.DeviceName = item.GetName();
                    * }}catch (SocketException){device.DeviceName = Utils.Common;}catch (Exception ex){var x = 0;}
            SetupNext(); successor?.HandleRequest(device); }

        private void SetupNext() { SetSuccessor(new PrintResultHandler()); }}}
*/

//        public override void HandleRequest(object request) {
//            var device = (FoundDevice)request;
//            try {
//                IPHostEntry entry = Dns.GetHostEntry(device.IpAddress);
//                device.DeviceName = (entry != null) ? entry.HostName : Utils.Common;
//            } catch (SocketException) {device.DeviceName = Utils.Common;}
//            if (!device.DeviceName.Equals(Utils.Common)){this.SetSuccessor(new PrintResultHandler());
//            }else{
//                this.SetSuccessor(new SshLookupHandler());
//                successor.SetSuccessor(new PrintResultHandler());
//            } successor?.HandleRequest(device); }}}

//if (!device.DeviceName.Equals(Utils.Common)) { } else {
//    //var factory = resolver(ServiceEnum.SNMP);
//    SetSuccessor(new SnmpHostNameHandler(resolver));
//    /*
//    var factory = resolver(ServiceEnum.SSH);
//    factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });

//    SetSuccessor(new SshLookupHandler(factory));
//    successor.SetSuccessor(new PrintResultHandler());
//    */
//}