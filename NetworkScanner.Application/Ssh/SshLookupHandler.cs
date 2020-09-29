/*
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System.Threading.Tasks;

namespace NetworkScanner.Application.Ssh
{
    /// <summary>
    /// Try to connect to provide and establish and SSH connection. If a connection can be
    /// established run the SetCommand.
    /// </summary>
    /// <seealso cref="AHandler"/>
    public class SshLookupHandler : AHandler
    {
        protected IRpcFactory Factory { get; }
        protected override Result Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SshLookupHandler"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public SshLookupHandler(IRpcFactory factory)
        {
            Factory = factory;
            Result = new Result { Value = Utils.Common, FoundAt = "" };
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override async Task HandleRequest(object request)
        {
            var device = (FoundDevice)request;

            var deviceId = Factory.FindValue(device.IpAddress, "Serial");//"cat /proc/cpuinfo"

            if (deviceId == null)
            {
                device.DeviceId = Utils.Common;
            }
            else
            {
                device.DeviceId = deviceId;
                //device.DeviceId = (deviceName.Equals(Utils.Common)) ? Utils.Common : deviceName;
            }

            successor?.HandleRequest(device);
        }
    }
}

/*
protected List<ConnectionOptions> SshConnections;
protected SshLookup SSH;
private string serialNumber;

public SshLookupHandler()
{
    SshConnections = new List<ConnectionOptions>();
    SSH = new SshLookup();

    serialNumber = Utils.Common;
}

public override void HandleRequest(object request)
{
    var device = (FoundDevice)request;
    LoadSshConnections(device.IpAddress);

    var sshResult = SshLookup("cat /proc/cpuinfo");
    if (sshResult != null)
        serialNumber = sshResult.Find(x => x.Key.Equals("Serial")).Value.Trim();

    device.DeviceId = serialNumber;

    //todo: is still N/A then call next check.

    successor?.HandleRequest(device);
}

protected void LoadSshConnections(string ip)
{
    //SshConnections
}

protected List<KeyValuePair<string, string>> SshLookup(string cmd)
{
    var pairs = new List<KeyValuePair<string, string>>();

    foreach (var connection in SshConnections)
    {
        pairs = SSH.FindValues(connection, cmd);

        if (pairs != null)
            break;
    }
    return pairs;
}
*/