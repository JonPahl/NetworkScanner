using PipleLineExample.LightBulb;

namespace NetworkScanner.Domain.LightBulb
{
    public class WizConfig : WizMsg
    {
        new public class Result
        {
            public string mac { get; set; }
            public int homeId { get; set; }
            public int roomId { get; set; }
            public bool homeLock { get; set; }
            public bool pairingLock { get; set; }
            public int typeId { get; set; }
            public string moduleName { get; set; }
            public string fwVersion { get; set; }
            public string groupId { get; set; }
            public int[] drvConf { get; set; }
        }
    }
}
