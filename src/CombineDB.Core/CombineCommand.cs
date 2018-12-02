namespace CombineDB.Core
{
    public abstract class CombineCommand<TModel, TResult> : CombineEvent<TModel, TResult>, ICombineCommand<TModel, TResult>
    {

    }
}
