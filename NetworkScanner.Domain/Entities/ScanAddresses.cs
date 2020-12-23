using System;

namespace NetworkScanner.Domain.Entities
{
    public class ScanAddresses
    {
        public DateTime Inserted { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }
    }
}
