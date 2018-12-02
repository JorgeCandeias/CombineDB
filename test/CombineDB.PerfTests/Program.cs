using CombineDB.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CombineDB.PerfTests
{
    public class Program
    {
        private static readonly Stopwatch watch = new Stopwatch();
        private static readonly CombineContext<SomeModel> context = new CombineContext<SomeModel>();

        public static void Main()
        {
            // test basic add command execution speed
            // these should run in sequence
            {
                var count = 10000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    tasks[i] = context.ExecuteAsync(new SomeAddCommand(new SomeEntity(i, 100)));
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}, SumOfValues = {model.SumOfValues}");
                Console.WriteLine($"Executed {count} commands at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }

            // test basic get query execution speed
            // these should run in parallel
            {
                var count = 10000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    tasks[i] = context.ExecuteAsync(new SomeGetQuery(i));
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}");
                Console.WriteLine($"Executed {count} queries at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }

            // test explicit projection query execution speed
            // these should run in parallel
            {
                var count = 10000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    tasks[i] = context.ExecuteAsync(new SomeSumQuery());
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}");
                Console.WriteLine($"Executed {count} queries at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }








            Console.WriteLine("Complete.");
            Console.ReadKey();
        }
    }

    public class SomeModel
    {
        public Dictionary<int, SomeEntity> Entities = new Dictionary<int, SomeEntity>();

        public decimal SumOfValues { get; set; }
    }

    public class SomeEntity
    {
        public SomeEntity(int id, decimal value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; }
        public decimal Value { get; }
    }

    public class SomeAddCommand : CombineCommand<SomeModel, SomeEntity>
    {
        public SomeAddCommand(SomeEntity entity)
        {
            Entity = entity;
        }

        public SomeEntity Entity { get; }

        protected override Task<SomeEntity> ApplyAsync(SomeModel model)
        {
            model.Entities[Entity.Id] = Entity;
            model.SumOfValues += Entity.Value;
            return Task.FromResult(Entity);
        }
    }

    public class SomeGetQuery : CombineQuery<SomeModel, SomeEntity>
    {
        public SomeGetQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

        protected override Task<SomeEntity> ApplyAsync(SomeModel model)
        {
            return Task.FromResult(model.Entities[Id]);
        }
    }

    public class SomeSumQuery : CombineQuery<SomeModel, decimal>
    {
        protected override Task<decimal> ApplyAsync(SomeModel model)
        {
            return Task.FromResult(model.SumOfValues);
        }
    }

    public class GetModelQuery : CombineQuery<SomeModel, SomeModel>
    {
        protected override Task<SomeModel> ApplyAsync(SomeModel model)
        {
            return Task.FromResult(model);
        }
    }
}
