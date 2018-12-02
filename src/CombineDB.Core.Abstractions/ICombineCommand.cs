namespace CombineDB.Core
{
    public interface ICombineCommand<TModel> : ICombineEvent<TModel>
    {
    }

    public interface ICombineCommand<TModel, TResult> :
        ICombineEvent<TModel, TResult>,
        ICombineCommand<TModel>
    {
    }
}
