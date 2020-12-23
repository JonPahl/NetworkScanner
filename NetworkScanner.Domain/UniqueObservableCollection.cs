using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetworkScanner.Domain
{
    /// <summary>
    /// Custom observable collection that only allows distinct values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UniqueObservableCollection<T> : ObservableCollection<T>
    {
        private Dictionary<int, T> _itemsDict = new Dictionary<int, T>();
        protected override void InsertItem(int index, T item)
        {
            var c = item.GetHashCode();
            if (_itemsDict.ContainsKey(c))
            {
                _itemsDict.Remove(c);
                _itemsDict.Add(c, item);
                base.InsertItem(index, item);
                return;
            }
            else
            {
                _itemsDict.Add(c, item);
                base.InsertItem(index, item);
            }
        }
        protected override void ClearItems()
        {
            _itemsDict.Clear();
            base.ClearItems();
        }
        protected override void RemoveItem(int index)
        {
            if (index >= base.Items.Count)
                return;
            var item = base.Items[index];
            if (!_itemsDict.ContainsKey(item.GetHashCode()))
                return;
            _itemsDict.Remove(item.GetHashCode());
            base.RemoveItem(index);
        }
    }
}
