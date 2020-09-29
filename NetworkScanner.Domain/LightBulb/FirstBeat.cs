using PipleLineExample.LightBulb;
using System.Text.Json.Serialization;

namespace NetworkScanner.Domain.LightBulb
{
    public class FirstBeat : WizMsg
    {
        //public string method { get; set; }
        //public string env { get; set; }

        [JsonPropertyName("params")]
        public Params _params { get; set; }

        public class Params
        {
            public string mac { get; set; }
            public int homeId { get; set; }
            public string fwVersion { get; set; }
        }
    }
}

//{"method":"firstBeat","env":"pro","params":{"mac":"a8bb508c7f98","homeId":1541638,"fwVersion":"1.19.3"}}