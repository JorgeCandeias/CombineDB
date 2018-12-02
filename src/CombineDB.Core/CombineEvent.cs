using System;
using System.Threading.Tasks;

namespace CombineDB.Core
{
    public abstract class CombineEvent<TModel, TResult> : ICombineEvent<TModel, TResult>
    {
        private TaskCompletionSource<TResult> _completionSource = new TaskCompletionSource<TResult>();

        public Task<TResult> Completion => _completionSource.Task;

        public async Task ExecuteAsync(TModel model)
        {
            try
            {
                var result = await ApplyAsync(model);
                _completionSource.SetResult(result);
            }
            catch (Exception e)
            {
                _completionSource.SetException(e);
                throw;
            }
        }

        protected abstract Task<TResult> ApplyAsync(TModel model);
    }
}
