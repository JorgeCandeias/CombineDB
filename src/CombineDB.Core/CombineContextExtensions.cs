using System;
using System.Threading.Tasks;

namespace CombineDB.Core
{
    public static class CombineContextExtensions
    {
        public static Task<TResult> ExecuteCommandAsync<TModel, TResult>(this ICombineContext<TModel> context, Func<TModel, TResult> action)
        {
            return context.ExecuteAsync(new QuickCommand<TModel, TResult>(model => Task.FromResult(action(model))));
        }

        public static Task<TResult> ExecuteCommandAsync<TModel, TResult>(this ICombineContext<TModel> context, Func<TModel, Task<TResult>> action)
        {
            return context.ExecuteAsync(new QuickCommand<TModel, TResult>(model => action(model)));
        }

        public static Task<TResult> ExecuteQueryAsync<TModel, TResult>(this ICombineContext<TModel> context, Func<TModel, TResult> action)
        {
            return context.ExecuteAsync(new QuickQuery<TModel, TResult>(model => Task.FromResult(action(model))));
        }

        public static Task<TResult> ExecuteQueryAsync<TModel, TResult>(this ICombineContext<TModel> context, Func<TModel, Task<TResult>> action)
        {
            return context.ExecuteAsync(new QuickQuery<TModel, TResult>(model => action(model)));
        }
    }
}
