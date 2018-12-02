using System.Threading.Tasks;

namespace CombineDB.Core
{
    public interface ICombineView<TSource>
    {
        void HandleItemAdded(TSource item);
        void HandleItemRemoved(TSource item);
    }

    public interface ICombineView<TSource, TView>
        : ICombineView<TSource>
    {

    }
}
