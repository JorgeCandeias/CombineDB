namespace CombineDB.Core
{
    public interface ICombineQuery<TModel> : ICombineEvent<TModel>
    {
    }

    public interface ICombineQuery<TModel, TResult> :
        ICombineEvent<TModel, TResult>,
        ICombineQuery<TModel>
    {
    }
}
