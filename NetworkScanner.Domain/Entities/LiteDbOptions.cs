namespace NetworkScanner.Domain.Entities
{
    public class LiteDbOptions
    {
        public string DatabaseLocation { get; set; }
        public string Connection { get; set; }
        public bool ReadOnly { get; set; }
    }
}