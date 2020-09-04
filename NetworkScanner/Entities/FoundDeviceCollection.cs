using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NetworkScanner.Entities
{
    public static class FoundDeviceCollection
    {
        public static event EventHandler<FoundDeviceChangedEventArgs> Changed;
        public static ConcurrentDictionary<int, FoundDevice> collection;

        public static bool Issynchronized { get; internal set; }

        static FoundDeviceCollection() => collection = new ConcurrentDictionary<int, FoundDevice>();

        /// <summary>
        /// Adds the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        public static void Add(FoundDevice newItem)
        {
            try
            {
                if (collection.Count > 0) // && collection.Values.Count(x => x.IpAddress == newItem.IpAddress) > 0
                {
                    var existingItem = collection.Values.FirstOrDefault(x => x.IpAddress == newItem.IpAddress);

                    if (existingItem != null)
                    {
                        var NewItemHashCode = newItem.GetHashCode();
                        var ExistingItemHashCode = existingItem.GetHashCode();

                        if (NewItemHashCode == ExistingItemHashCode)
                        {
                            //same, update to track last seen.
                            if (collection.TryUpdate(newItem.GetHashCode(), newItem, existingItem))
                                Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Replaced, newItem, null));
                        }
                        else
                        {
                            if (newItem.DeviceId != "" && !newItem.DeviceId.Contains("N/A"))
                            {
                                if (collection.TryRemove(existingItem.GetHashCode(), out FoundDevice removed))
                                {
                                    if (collection.TryAdd(newItem.GetHashCode(), newItem))
                                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                                }
                                else
                                {
                                    if (!collection.ContainsKey(newItem.GetHashCode()))
                                    {
                                        if (collection.TryAdd(newItem.GetHashCode(), newItem))
                                        {
                                            Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (collection.TryAdd(newItem.GetHashCode(), newItem))
                            Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                    }

                    /*
                    if (existingItem != null && collection.TryUpdate(newItem.GetHashCode(), newItem, existingItem))
                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Replaced, newItem, null));
                    */
                }
                else
                {
                    if (collection.TryAdd(newItem.GetHashCode(), newItem))
                        Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
                }
            }
            catch (ArgumentException)
            {
                if (collection.TryUpdate(newItem.GetHashCode(), newItem, newItem))
                {
                    Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Replaced, newItem, null));
                }//suppress duplicate key exception
            }
        }
    }

    public class FoundDeviceChangedEventArgs : EventArgs
    {
        private FoundDevice _changedItem;
        private ChangeType _changeType;
        private FoundDevice _replacedWith;

        public FoundDevice ChangedItem { get { return _changedItem; } }
        public ChangeType ChangeType { get { return _changeType; } }
        public FoundDevice ReplacedWith { get { return _replacedWith; } }

        public FoundDeviceChangedEventArgs(ChangeType change, FoundDevice item, FoundDevice replacement)
        {
            _changeType = change;
            _changedItem = item;
            _replacedWith = replacement;
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