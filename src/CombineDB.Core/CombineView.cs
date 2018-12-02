using System;

namespace CombineDB.Core
{
    public class CombineView<TSource, TView> :
        ICombineView<TSource, TView>
        where TView : new()
    {
        public CombineView(ICombineSet<TSource> source, Func<TView, TSource, TView> add, Func<TView, TSource, TView> remove)
        {
            _add = add;
            _remove = remove;

            source.Subscribe(this);
        }

        private readonly Func<TView, TSource, TView> _add;
        private readonly Func<TView, TSource, TView> _remove;

        public TView View { get; private set; } = new TView();

        public void HandleItemAdded(TSource item)
        {
            View = _add(View, item);
        }

        public void HandleItemRemoved(TSource item)
        {
            View = _remove(View, item);
        }
    }
}
