using System;
using System.Collections.Generic;

namespace CombineDB.Core
{
    public class CombineSet<T, K> : ICombineSet<T, K>
    {
        private readonly Func<T, K> _keyExtractor;
        private readonly Dictionary<K, T> _data = new Dictionary<K, T>();

        public CombineSet(Func<T, K> keyExtractor)
        {
            _keyExtractor = keyExtractor;
        }

        public T Get(K key)
        {
            return _data[key];
        }

        public T GetOrDefault(K key)
        {
            if (_data.TryGetValue(key, out T value))
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }

        public void Set(T item)
        {
            _data[_keyExtractor(item)] = item;
        }

    }
}
