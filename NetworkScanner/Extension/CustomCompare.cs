using NetworkScanner.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetworkScanner.Extension
{
    public class EqualityCompare : IEqualityComparer<FoundDevice>
    {
        public bool Equals(FoundDevice newItem, FoundDevice oldItem)
        {
            return newItem.DeviceId != oldItem.DeviceId;
        }

        public int GetHashCode([DisallowNull] FoundDevice obj)
        {
            obj.FoundAt = null;
            return obj.GetHashCode();
        }
    }
}