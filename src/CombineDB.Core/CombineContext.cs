using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CombineDB.Core
{
    public class CombineContext<TModel> : ICombineContext<TModel> where TModel : class, new()
    {
        private readonly TModel _model;
        private readonly ITargetBlock<ICombineEvent<TModel>> _flow;

        #region Flow State

        private readonly Queue<Task> _commands = new Queue<Task>();
        private readonly Queue<Task> _queries = new Queue<Task>();

        #endregion

        public CombineContext()
        {
            _model = new TModel();

            _flow = new ActionBlock<ICombineEvent<TModel>>(async e =>
            {
                switch (e)
                {
                    case ICombineCommand<TModel> command:

                        // wait for previous commands to complete
                        while (_commands.Count > 0)
                        {
                            await _commands.Dequeue();
                        }

                        // wait for previous queries to complete
                        while (_queries.Count > 0)
                        {
                            await _queries.Dequeue();
                        }

                        // execute this command now
                        _commands.Enqueue(command.ExecuteAsync(_model));
                        break;

                    case ICombineQuery<TModel> query:

                        // wait for previous command to complete
                        while (_commands.Count > 0)
                        {
                            await _commands.Dequeue();
                        }

                        // execute this query along with other queries
                        _queries.Enqueue(query.ExecuteAsync(_model));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            });
        }

        public Task<TResult> ExecuteAsync<TResult>(ICombineEvent<TModel, TResult> evt)
        {
            _flow.Post(evt);
            return evt.Completion;
        }
    }
}
