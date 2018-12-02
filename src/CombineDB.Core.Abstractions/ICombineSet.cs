namespace CombineDB.Core
{
    public interface ICombineSet
    {
    }

    public interface ICombineSet<T, K> : ICombineSet
    {
        void Set(T item);
        T Get(K key);
        T GetOrDefault(K key);
    }
}
