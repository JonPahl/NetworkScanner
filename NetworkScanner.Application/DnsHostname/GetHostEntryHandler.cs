/*
using NetworkScanner.Application.Common;
using NetworkScanner.Application.SNMP;
using NetworkScanner.Application.WizBulb;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkScanner.Application.DnsHostname {
    /// <summary>
    /// Check for DNS HostName.
    /// </summary>
    /// <seealso cref="Handler"/>
    public class GetHostEntryHandler : AHandler {
        private readonly Func<ServiceEnum, ARpcFactory> resolver;
        #region Move up
        protected SnmpHostNameHandler smp;
        protected WizBulbHandler wizBulb;
        protected override Result Result { get; set; }
        #endregion Move up
        public GetHostEntryHandler(Func<ServiceEnum, ARpcFactory> serviceResolver){
            resolver = serviceResolver;
            smp = new SnmpHostNameHandler(resolver);
            wizBulb = new WizBulbHandler(resolver);
            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        public override Task<Result> HandleRequestWithResult(object request){
            var device = (FoundDevice)request;
            try{
                IPHostEntry entry = Dns.GetHostEntry(device.IpAddress);
                var value = entry != null ? entry.HostName : Utils.Common;
                Result.FoundAt = "HostEntry";
                Result.Value = value;
                return Task.Run(() => Result);
            }
            catch (SocketException)
            {
                Result = new Result { Value = Utils.Common, FoundAt = "" };
                return Task.Run(() => Result);
            }
            catch (Exception ex)
            {
                var z = ex.Message;
                Result = new Result { Value = Utils.Common, FoundAt = "" };
                return Task.Run(() => Result);
            }
        }

        public override async Task HandleRequest(object request)
        {
            //var device = (FoundDevice)request;

            //try {
            //    // if (device.DeviceName.Equals(Utils.Common)) {
            //    var tasks = new List<Task<object>>
            //    {
            //        smp.HandleRequestWithResult(device),
            //        wizBulb.HandleRequestWithResult(device)
            //    };

            // var taskResults = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
            // var result = Utils.Common;
            // var found = taskResults.Where(x => !x.Equals(Utils.Common)).ToList();
            // if (found.Count > 0) result = found.FirstOrDefault().ToString();
            // device.DeviceName = result;
            // //var x = 0; //}
            //}
            //catch (SocketException)
            //{
            //    device.DeviceName = Utils.Common;
            //}
            ////return await (await Task.Run(() => Utils.Common).ConfigureAwait(false) as Task<T>);
            //SetupNext(device);
            //successor?.HandleRequest(device);
        }


        /*
        private void SetupNext(FoundDevice device)
        {
            //if (!device.DeviceName.Equals(Utils.Common)) {
            SetSuccessor(new PrintResultHandler());
            //} else {
            //    //var factory = resolver(ServiceEnum.SNMP);
            //    SetSuccessor(new SnmpHostNameHandler(resolver));

            // var factory = resolver(ServiceEnum.SSH); factory.SetCommands(new List<object> { "cat /proc/cpuinfo" });
            //    SetSuccessor(new SshLookupHandler(factory));
            //    successor.SetSuccessor(new PrintResultHandler());
            //}
        }       
    }
}

// public override void HandleRequest(object request) { var device = (FoundDevice)request;

// try { IPHostEntry entry = Dns.GetHostEntry(device.IpAddress); device.DeviceName = (entry != null)
// ? entry.HostName : Utils.Common; } catch (SocketException) { device.DeviceName = Utils.Common; }

//            if (!device.DeviceName.Equals(Utils.Common))
//            {
//                this.SetSuccessor(new PrintResultHandler());
//            }
//            else
//            {
//                this.SetSuccessor(new SshLookupHandler());
//                successor.SetSuccessor(new PrintResultHandler());
//            }
//            successor?.HandleRequest(device);
//        }
//    }
//}
*/