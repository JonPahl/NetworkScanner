using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace NetworkScanner.Entities
{
    public static class FoundDeviceCollection
    {
        private static readonly ReaderWriterLock rwl = new ReaderWriterLock();
        public static ObservableCollection<FoundDevice> collection = new ObservableCollection<FoundDevice>();

        public static void Add(FoundDevice device)
        {
            try
            {
                rwl.AcquireWriterLock(3000);
                try
                {
                    if (collection.Count == 0)
                        collection.Add(device);

                    if(device.Equals(device))
                    {
                        collection.Add(device);
                    }

                    /*
                    if (device.DeviceId == null && !collection.Any(x => x.IpAddress == device.IpAddress))
                    {
                        collection.Add(device);
                    }

                    if (device.DeviceId != null && !collection.Any(x => x.DeviceId != device.DeviceId))
                    {
                        if (!collection.Any(x => x.IpAddress != device.IpAddress))
                        {
                            if (!collection.Any(x => x.DeviceName != device.DeviceName))
                            {
                                collection.Add(device);
                            }
                        }
                    }
                    */

                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                //Interlocked.Increment(ref writerTimeouts);
            }
        }
    }
}
