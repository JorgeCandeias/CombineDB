using System;
using System.Threading.Tasks;

namespace CombineDB.Core
{
    public class QuickCommand<TModel, TResult> : CombineCommand<TModel, TResult>
    {
        private readonly Func<TModel, Task<TResult>> _expression;

        public QuickCommand(Func<TModel, Task<TResult>> expression)
        {
            _expression = expression;
        }

        protected override Task<TResult> ApplyAsync(TModel model)
        {
            return _expression(model);
        }
    }
}
