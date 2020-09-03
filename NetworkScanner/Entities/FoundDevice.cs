using System;

namespace NetworkScanner.Entities
{
    public class FoundDevice //: IComparable<FoundDevice>
    {
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string FoundUsing { get; set; }
        public DateTime? FoundAt { get; set; }

        public FoundDevice()
        {
            IpAddress = string.Empty;
            DeviceId = string.Empty;
            DeviceName = string.Empty;
            FoundUsing = string.Empty;
            FoundAt = DateTime.Now;
        }

        public override int GetHashCode()
        {
            var custom = $"{IpAddress}_{DeviceName}_{DeviceId}";
            return custom.GetHashCode();
            //return base.GetHashCode();
        }

        /*
        public int CompareTo([AllowNull] FoundDevice other)
        {
            if (other == null)
                return -1;

            if (other.DeviceId == null)
            {
                if (this.IpAddress != other.IpAddress)
                {
                    return 1;
                }
                else
                {
                    if (this.DeviceName != other.DeviceName)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            else
            {
                if (this.DeviceId != other.DeviceId)
                {
                    return 1;
                }
                else
                {
                    if (this.IpAddress != other.IpAddress)
                    {
                        return -1;
                    }
                    else
                    {
                        if (this.DeviceName != other.DeviceName)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }

                    //   var result = (this.DeviceId == other.DeviceId && this.DeviceName == other.DeviceName && this.IpAddress == other.IpAddress);
                    // return !result;
                }
            }
        }

        public bool Equals([AllowNull] FoundDevice other)
        {
            if (other == null)
                return false;

            if (other.DeviceId == null)
            {
                if (this.IpAddress != other.IpAddress)
                {
                    return true;
                }
                else
                {
                    return this.DeviceName != other.DeviceName;
                }
            }
            else
            {
                if (this.DeviceId != other.DeviceId)
                {
                    return true;
                }
                else
                {
                    if (this.IpAddress != other.IpAddress)
                    {
                        return true;
                    }
                    else
                    {
                        if (this.DeviceName != other.DeviceName)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //   var result = (this.DeviceId == other.DeviceId && this.DeviceName == other.DeviceName && this.IpAddress == other.IpAddress);
                    // return !result;
                }
            }
        }


        */

    }
}