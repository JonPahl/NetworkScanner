namespace PipleLineExample.LightBulb
{
    public class WizMsg
    {
        public string method { get; set; }
        public string env { get; set; }
        public Result result { get; set; }

        public class Result
        {
            public string mac { get; set; }
            public int rssi { get; set; }
            public string src { get; set; }
            public bool state { get; set; }
            public int sceneId { get; set; }
            public int speed { get; set; }
            public int dimming { get; set; }
        }
    }
}
