using ObjectsComparer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Domain.Entities
{
    public static class FoundDeviceCollection
    {
        public static event EventHandler<FoundDeviceChangedEventArgs> Changed;
        public static ConcurrentDictionary<int, FoundDevice> collection
            = new ConcurrentDictionary<int, FoundDevice>();

        private static readonly ObjectsComparer.Comparer<FoundDevice> comparer
            = new ObjectsComparer.Comparer<FoundDevice>();

        public static bool Issynchronized { get; internal set; }

        /// <summary>
        /// Adds the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        public static void Add(FoundDevice newItem)
        {
            try
            {
                if (collection.Count == 0)
                {
                    if (collection.TryAdd(newItem.GenerateId(), newItem))
                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                }
                else
                {
                    var existingItem = collection.Values.FirstOrDefault(x => x.IpAddress.Equals(newItem.IpAddress));

                    if (existingItem == null)
                    {
                        if (collection.TryAdd(newItem.GenerateId(), newItem))
                            Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                    }
                    else
                    {
                        bool isEqual = comparer.Compare(newItem, existingItem, out IEnumerable<Difference> differences);

                        if (isEqual)
                        {
                            if (collection.TryUpdate(newItem.GenerateId(), newItem, existingItem))
                            {
                                Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Replaced, newItem, null));
                            }
                            else
                            {
                                MergeRecord(existingItem.GenerateId(), newItem);
                            }
                        }
                        else
                        {
                            var deviceId = differences
                                .FirstOrDefault(x => x.MemberPath.Equals("DeviceId"));
                            if (deviceId == null)
                            {
                                if (collection.TryUpdate(existingItem.GenerateId(), newItem, existingItem))
                                    Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Replaced, newItem, null));
                            }
                            else
                            {
                                MergeRecord(existingItem.GenerateId(), newItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void MergeRecord(int existingCode, FoundDevice newItem)
        {
            try
            {
                if (collection.TryRemove(existingCode, out FoundDevice removed))
                {
                    if (collection.TryAdd(newItem.GenerateId(), newItem))
                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                }
                else
                {
                    if (!collection.ContainsKey(newItem.GenerateId()) && collection.TryAdd(newItem.GenerateId(), newItem))
                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class FoundDeviceChangedEventArgs : EventArgs
    {
        public FoundDevice ChangedItem { get; }
        public ChangeType ChangeType { get; }
        public FoundDevice ReplacedWith { get; }

        public FoundDeviceChangedEventArgs(ChangeType change, FoundDevice item, FoundDevice replacement)
        {
            ChangeType = change;
            ChangedItem = item;
            ReplacedWith = replacement;
        }
    }
    public enum ChangeType
    {
        Added,
        Removed,
        Replaced,
        Cleared
    }
}