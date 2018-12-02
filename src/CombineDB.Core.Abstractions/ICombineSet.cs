using System;

namespace CombineDB.Core
{
    public interface ICombineSet<T>
    {
        void Subscribe(ICombineView<T> view);
    }

    public interface ICombineSet<T, TKey> : ICombineSet<T>
    {
        T Get(TKey key);
        void Add(T item);
        void Set(T item);
        bool Remove(TKey key);
        int Count { get; }
    }
}
