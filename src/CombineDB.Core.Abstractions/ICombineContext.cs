using System.Threading.Tasks;

namespace CombineDB.Core
{
    public interface ICombineContext<TModel>
    {
        Task<TResult> ExecuteAsync<TResult>(ICombineEvent<TModel, TResult> evt);
    }
}
