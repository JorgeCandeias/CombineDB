namespace CombineDB.Core
{
    public abstract class CombineQuery<TModel, TResult> : CombineEvent<TModel, TResult>, ICombineQuery<TModel, TResult>
    {
    }
}
