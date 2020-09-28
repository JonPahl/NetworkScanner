using System;
using System.Text;

namespace NetworkScanner.Domain.Entities
{
    public class FoundDevice //: Details
    {
        //Details
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public DateTime? FoundAt { get; set; }
        public string FoundUsing { get; set; }
        public object Key { get; set; }

        public FoundDevice()
        {
            DeviceId = string.Empty;
            DeviceName = string.Empty;
            FoundUsing = string.Empty;
            FoundAt = DateTime.Now;
        }

        public override int GetHashCode()
        {
            try
            {
                var txt = $"{DeviceId}_{DeviceName}";
                return txt.GetHashCode(StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return 0;
            }
        }

        public int GenerateId()
        {
            if (!DeviceId.Equals(Utils.Common))
            {
                return DeviceId.GetHashCode();
            }
            else
            {
                return IpAddress.GetHashCode();
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach(var prop in this.GetType().GetProperties())
            {
                str.Append(prop.GetValue(this));
            }
            return str.ToString();
        }
    }

    public class Details
    {
        public Details()
        {
            Cnt = 3;
        }

        public int Cnt { get; set; }
    }
}
