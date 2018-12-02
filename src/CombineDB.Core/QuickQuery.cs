using System;
using System.Threading.Tasks;

namespace CombineDB.Core
{
    public class QuickQuery<TModel, TResult> : CombineQuery<TModel, TResult>
    {
        private readonly Func<TModel, Task<TResult>> _expression;

        public QuickQuery(Func<TModel, Task<TResult>> expression)
        {
            _expression = expression;
        }

        protected override Task<TResult> ApplyAsync(TModel model)
        {
            return _expression(model);
        }
    }
}
