using System.Threading.Tasks;

namespace CombineDB.Core
{
    public interface ICombineEvent<TModel>
    {
        Task ExecuteAsync(TModel model);
    }

    public interface ICombineEvent<TModel, TResult> : ICombineEvent<TModel>
    {
        Task<TResult> Completion { get; }
    }
}
