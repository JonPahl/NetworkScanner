using PipleLineExample.LightBulb;
using System.Text.Json.Serialization;

namespace NetworkScanner.Domain.LightBulb
{
    public class FirstBeat : WizMsg
    {
        [JsonPropertyName("params")]
        public Params _Params { get; set; }

        public class Params
        {
            public string mac { get; set; }
            public int homeId { get; set; }
            public string fwVersion { get; set; }
        }
    }
}