using System;
using System.Collections.Generic;

namespace CombineDB.Core
{
    public class CombineSet<T, TKey> : ICombineSet<T, TKey>
    {
        private readonly Func<T, TKey> _keyExtractor;
        private readonly Dictionary<TKey, T> _data = new Dictionary<TKey, T>();

        public CombineSet(Func<T, TKey> keyExtractor)
        {
            _keyExtractor = keyExtractor;
        }

        public T Get(TKey key)
        {
            return _data[key];
        }

        public void Add(T item)
        {
            _data.Add(_keyExtractor(item), item);
            foreach (var view in _subscribers)
            {
                view.HandleItemAdded(item);
            }
        }

        public void Set(T item)
        {
            Remove(_keyExtractor(item));
            Add(item);
        }

        public bool Remove(TKey key)
        {
            if (_data.TryGetValue(key, out var item))
            {
                _data.Remove(key);
                foreach (var view in _subscribers)
                {
                    view.HandleItemRemoved(item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private readonly HashSet<ICombineView<T>> _subscribers = new HashSet<ICombineView<T>>();
        public void Subscribe(ICombineView<T> view)
        {
            _subscribers.Add(view);
        }

        public int Count => _data.Count;
    }
}
