/*
using NetworkScanner.Domain.Entities;
using System.Threading.Tasks;

namespace NetworkScanner.Application
{
    /// <summary>
    /// Print out results.
    /// </summary>
    /// <seealso cref="Handler"/>
    public class PrintResultHandler : AHandler
    {
        public PrintResultHandler()
        {
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
            device.Id = "0";

            FoundDeviceCollection.Add(device);

            //var devices = new List<FoundDevice> { device };
            //Console.WriteLine(DisplayTable(devices.ToList()));

            //Console.WriteLine("{0}: \t {1} \t{2}", device.IpAddress, device.DeviceName, device.DeviceId);
            //return Task.CompletedTask;
            //return await (await Task.Run(() => Utils.Common).ConfigureAwait(false) as Task<T>);

            //Result = new Result();
        }

        /*
        private static void BuildTable()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            var devices = FoundDeviceCollection
                .collection
                .Select(x => x.Value)
                .OrderBy(x => x, new ObjectCompare());

            var i = 1;
            foreach (var device in devices)
            {
                device.Id = i;
                i++;
            }

            Console.WriteLine(DisplayTable(devices.ToList()));
        }
        */

        /*
        private static string DisplayTable(List<FoundDevice> devices)
        {
            return devices.ToStringTable(
                              new[] { "ID", "IP ADDRESS", "DEVICE NAME", "DEVICE ID", "FOUND USING", "TIMESTAMP" },
                              a => a.Id, a => a.IpAddress, a => a.DeviceName, a => a.DeviceId, a => a.FoundUsing, a => a.FoundAt);
        }
        *
    }
}
*/