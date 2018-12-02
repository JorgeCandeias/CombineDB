using System;

namespace CombineDB.Core
{
    public class CombineSetChangedEventArgs<T> : EventArgs
    {
        public CombineSetChangedEventArgs(CombineSetChangeType changeType, T item)
        {
            ChangeType = changeType;
            Item = item;
        }

        public CombineSetChangeType ChangeType { get; }

        public T Item { get; }
    }
}
