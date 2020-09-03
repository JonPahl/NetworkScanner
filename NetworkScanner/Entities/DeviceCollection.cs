//using NetworkScanner.Extension;
//using System;
//using System.Collections;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;

//namespace NetworkScanner.Entities {
//    public class DeviceCollection<FoundDevice> : IProducerConsumerCollection<FoundDevice> {
//        // Used for enforcing thread-safety
//        private object m_lockObject = new object();

//        // We'll use a regular old Stack for our core operations
//        private Stack<FoundDevice> sequentialList = null;
//        public DeviceCollection(IEnumerable<FoundDevice> collection) { sequentialList = new List<FoundDevice>(); }
//        public DeviceCollection() { }

//        // Safe Push/Pop support
//        //
//        public void Push(FoundDevice item)
//        {
//            var comparer = new EqualityCompare();

//            lock (m_lockObject)
//            {
//                //var found = sequentialList.Where(x=> x.DeviceId == item.DeviceId);
//                //var found = l.FirstOr<FoundDevice>(x => x.DeviceId == y.DeviceId);

//                //var m = this.GetEnumerator() as IEnumerable<FoundDevice>;  //ToArray();
//                //if (found != null)
//                //{
//                //    //var z = m.Any(x => x.DeviceId == y.DeviceId);
//                //    var d = 0;
//                //    sequentialList.Add(item); // .Push(item);
//                //} else
//                //{
//                sequentialList.Push(item);
//                //}

//                //foreach (T x in m_sequentialStack)
//                //{
//                //    var z = x as FoundDevice;
//                //    if (z.DeviceId == y.DeviceId)
//            }

//            var r = m_sequentialStack.Contains<FoundDevice>(item, comparer);  //.Contains<FoundDevice>(y, comparer);

//            /*
//            var duplicateCount = m_sequentialStack.Where<FoundDevice>(x => x.DeviceId.Equals(item.DeviceId)).Count();

//            if (duplicateCount != 0) return;

//            collection.Push(newItem);
//            Changed?.Invoke(newItem, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
//            */
//        }

//        public bool TryPop(out FoundDevice item)
//        {
//            bool rval = true;
//            lock (m_lockObject)
//            {
//                if (sequentialList.Count == 0) { item = default(FoundDevice); rval = false; }
//                else
//                {
//                    item = sequentialList.First<FoundDevice>();
//                }
//            }
//            return rval;
//        }

//        public bool TryTake(out FoundDevice item)
//        {
//            return TryPop(out item);
//        }

//        public bool TryAdd(FoundDevice item)
//        {
//            Push(item);
//            return true; // Push doesn't fail
//        }

//        public FoundDevice[] ToArray()
//        {
//            FoundDevice[] rval = null;
//            lock (m_lockObject) rval = sequentialList.ToArray();
//            return rval;
//        }

//        public void CopyTo(FoundDevice[] array, int index)
//        {
//            lock (m_lockObject) sequentialList.CopyTo(array, index);
//        }

//        //
//        // Support for IEnumerable(T)
//        //
//        public IEnumerator<FoundDevice> GetEnumerator()
//        {
//            // The performance here will be unfortunate for large stacks,
//            // but thread-safety is effectively implemented.
//            Stack<FoundDevice> stackCopy = null;
//            lock (m_lockObject) stackCopy = new Stack<FoundDevice>(sequentialList);
//            //return stackCopy;
//            //return sequentialList.GetEnumerator();
//        }

//        //
//        // Support for IEnumerable
//        //
//        //IEnumerator .GetEnumerator()
//        //{
//        //    return ((IEnumerable<FoundDevice>)this).GetEnumerator();
//        //}

//        //
//        // Support for ICollection
//        //
//        public bool IsSynchronized => true;

//        public object SyncRoot => m_lockObject;

//        public int Count => sequentialList.Count;

//        public void CopyTo(Array array, int index)
//        {
//            lock (m_lockObject) ((ICollection)sequentialList).CopyTo(array, index);
//        }
//    }
//}

///*
//        public event EventHandler<FoundDeviceChangedEventArgs> Changed;

//        public DeviceCollection() : base() { }

//        /// <summary>
//        /// This is the only method that absolutely must be overridden, because without it the KeyedCollection cannot extract the
//        // keys from the items. The input parameter type is the second generic type argument, in this case FoundDevice, and
//        // the return value type is the first generic type argument, in this case int.
//        /// </summary>
//        /// <param name="item">The element from which to extract the key.</param>
//        /// <returns>
//        /// The key for the specified element.
//        /// </returns>
//        protected override string GetKeyForItem(FoundDevice item)
//        {
//            //if (item == null)
//            //{
//            //    // In this example, the key is the part number.
//                return item.DeviceId;
//            //}
//            //else
//            //{
//            //    return "N/A";
//            //}
//        }

//        protected virtual void OnChanged(FoundDeviceChangedEventArgs e)
//        {
//            Changed?.Invoke(this, e);
//        }

//        protected override void InsertItem(int index, FoundDevice newItem)
//        {
//            lock (this)
//            {
//                try
//                {
//                    var i = base.Items.Count;
//                    base.InsertItem(index, newItem);

//                    Changed?.Invoke(this, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
//                }
//                catch (ArgumentException)
//                {
//                }
//                catch (Exception ex)
//                {
//                    var e = ex.GetType();
//                    var x = ex.Message;
//                }
//            }
//        }

//        /*
//        protected virtual void OnChanged(FoundDeviceChangedEventArgs e)
//        {
//            Changed?.Invoke(this, e);
//        }

//        protected override void InsertItem(int index, FoundDevice newItem)
//        {
//            try
//            {
//                var i = base.Items.Count;
//                base.InsertItem(i++, newItem);

//                EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
//                if (temp != null)
//                    temp(this, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
//            }
//            catch (ArgumentException)
//            {
//            }
//            catch (Exception ex)
//            {
//                var e = ex.GetType();
//                var x = ex.Message;
//            }
//        }

//        protected override void SetItem(int index, FoundDevice newItem)
//        {
//            FoundDevice replaced = Items[index];
//            base.SetItem(index, newItem);

//            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
//            if (temp != null)
//                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Replaced, replaced, newItem));
//        }

//        protected override void RemoveItem(int index)
//        {
//            FoundDevice removedItem = Items[index];
//            base.RemoveItem(index);

//            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
//            if (temp != null)
//            {
//                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Removed, removedItem, null));
//            }
//        }

//        protected override void ClearItems()
//        {
//            base.ClearItems();

//            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
//            if (temp != null)
//                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Cleared, null, null));
//        }
//        */
////}

///*
//public class FoundDeviceChangedEventArgs : EventArgs
//{
//    private FoundDevice _changedItem;
//    private ChangeType _changeType;
//    private FoundDevice _replacedWith;

//    public FoundDevice ChangedItem { get { return _changedItem; } }
//    public ChangeType ChangeType { get { return _changeType; } }
//    public FoundDevice ReplacedWith { get { return _replacedWith; } }

//    public FoundDeviceChangedEventArgs(ChangeType change, FoundDevice item, FoundDevice replacement)
//    {
//        _changeType = change;
//        _changedItem = item;
//        _replacedWith = replacement;
//    }}
//public enum ChangeType{Added,Removed,Replaced,Cleared}
//}

//            /*

////using System;
////using System.Collections.ObjectModel;

////namespace NetworkScanner.Entities
////{
////    public class DeviceCollection : KeyedCollection<string, FoundDevice>
////    {
////        public event EventHandler<FoundDeviceChangedEventArgs> Changed;
////        public DeviceCollection()

////        /// <summary>
////        /// This is the only method that absolutely must be overridden, because without it the KeyedCollection cannot extract the
////        // keys from the items. The input parameter type is the second generic type argument, in this case FoundDevice, and
////        // the return value type is the first generic type argument, in this case int.
////        /// </summary>
////        /// <param name="item">The element from which to extract the key.</param>
////        /// <returns>
////        /// The key for the specified element.
////        /// </returns>
////        protected override string GetKeyForItem(FoundDevice item)
////        {
////            if (item == null)
////            {
////                // In this example, the key is the part number.
////                return item.DeviceId;
////            } else
////            {
////                return "N/A";
////            }
////        }

////        protected virtual void OnChanged(FoundDeviceChangedEventArgs e)
////        {
////            Changed?.Invoke(this, e);
////        }

////        protected override void InsertItem(int index, FoundDevice newItem)
////        {
////            try
////            {
////                var i = base.Items.Count;
////                base.InsertItem(i++, newItem);

////                EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
////                if (temp != null)
////                    temp(this, new FoundDeviceChangedEventArgs(ChangeType.Added, newItem, null));
////            }
////            catch(ArgumentException)
////            {
////            }
////            catch (Exception ex)
////            {
////                var e = ex.GetType();
////                var x = ex.Message;
////            }
////        }

////        protected override void SetItem(int index, FoundDevice newItem)
////        {
////            FoundDevice replaced = Items[index];
////            base.SetItem(index, newItem);

////            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
////            if (temp != null)
////                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Replaced, replaced, newItem));
////        }

////        protected override void RemoveItem(int index)
////        {
////            FoundDevice removedItem = Items[index];
////            base.RemoveItem(index);

////            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
////            if (temp != null)
////            {
////                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Removed, removedItem, null));
////            }
////        }

////        protected override void ClearItems()
////        {
////            base.ClearItems();

////            EventHandler<FoundDeviceChangedEventArgs> temp = Changed;
////            if (temp != null)
////            {
////                temp(this, new FoundDeviceChangedEventArgs(ChangeType.Cleared, null, null));
////            }
////        }
////    }

////    public class FoundDeviceChangedEventArgs : EventArgs{
////        private FoundDevice _changedItem;
////        private ChangeType _changeType;
////        private FoundDevice _replacedWith;
////        public FoundDevice ChangedItem { get { return _changedItem; } }
////        public ChangeType ChangeType { get { return _changeType; } }
////        public FoundDevice ReplacedWith { get { return _replacedWith; } }
////        public FoundDeviceChangedEventArgs(ChangeType change, FoundDevice item, FoundDevice replacement)
////        {
////            _changeType = change;
////            _changedItem = item;
////            _replacedWith = replacement;
////        }}
////    public enum ChangeType{Added,Removed,Replaced,Cleared}}
