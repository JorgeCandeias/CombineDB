using CombineDB.Core;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CombineDB.PerfTests
{
    public class Program
    {
        private static readonly Stopwatch watch = new Stopwatch();
        private static readonly CombineContext<SomeModel> context = new CombineContext<SomeModel>();

        public static void Main()
        {
            /*
            // test basic add command execution speed
            // these should run in sequence
            {
                var count = 1000000;
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


            // test ad-hoc query execution speed
            // these should run in parallel
            {
                var count = 1000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    tasks[i] = context.ExecuteAsync(new QuickQuery<SomeModel, decimal>(m => Task.FromResult(m.Entities.Take(10).Sum(x => x.Value.Value))));
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}");
                Console.WriteLine($"Executed {count} ad-hoc queries at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }
            */

            // test basic ad-hoc command execution locking
            // these should run in sequence with a visible interval
            {
                Console.WriteLine("Testing Ad-Hoc Commands on CombineSet...");
                var count = 1000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    var id = i;
                    tasks[i] = context.ExecuteCommandAsync(m =>
                    {
                        m.SetEntities.Add(new SomeEntity(id, id));
                        return 0;
                    });
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.SetEntities.Count}, SumOfValues = {model.SumOfValuesView.View}");
                Console.WriteLine($"Executed {count} Adds on CombineSet at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }




            // test custom command execution speed
            // these should run in sequence
            {
                var count = 1000000;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    var id = i;
                    tasks[i] = context.ExecuteAsync(new SomeAddCommand(new SomeEntity(id, id)));
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}, SumOfValues = {model.SumOfValues}");
                Console.WriteLine($"Executed {count} custom commands at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }

            // test custom query execution speed
            // these should run in parallel
            {
                var count = 1000000;
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




            // test basic ad-hoc command execution locking
            // these should run in sequence with a visible interval
            {
                Console.WriteLine("Testing Ad-Hoc Command Blocking...");
                var count = 10;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    var id = i;
                    tasks[i] = context.ExecuteCommandAsync(async m =>
                    {
                        await Task.Delay(500);
                        Console.WriteLine("Ad-Hoc Command Executed.");
                        return 1000;
                    });
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}, SumOfValues = {model.SumOfValues}");
                Console.WriteLine($"Executed {count} ad-hoc commands at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }

            // test basic ad-hoc query execution blocking
            // these should run in parallel with no visible interval
            {
                Console.WriteLine("Testing Ad-Hoc Query Blocking...");
                var count = 10;
                var tasks = new Task[count];
                watch.Restart();
                for (var i = 0; i < count; ++i)
                {
                    var id = i;
                    tasks[i] = context.ExecuteQueryAsync(async m =>
                    {
                        await Task.Delay(500);
                        Console.WriteLine("Ad-Hoc Query Executed");
                        return 1000;
                    });
                }
                Task.WaitAll(tasks);
                watch.Stop();

                var model = context.ExecuteAsync(new GetModelQuery()).Result;
                Console.WriteLine($"Entities = {model.Entities.Count}, SumOfValues = {model.SumOfValues}");
                Console.WriteLine($"Executed {count} ad-hoc queries at a rate of {count / watch.Elapsed.TotalSeconds:N0}/s");
            }








            // test explicit projection query execution speed
            // these should run in parallel
            /*
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
            */







            Console.WriteLine("Complete.");
            Console.ReadKey();
        }
    }

    public class SomeModel
    {
        public ConcurrentDictionary<int, SomeEntity> Entities { get; } = new ConcurrentDictionary<int, SomeEntity>();
        public decimal SumOfValues { get; set; }


        public ICombineSet<SomeEntity, int> SetEntities { get; } = new CombineSet<SomeEntity, int>(x => x.Id);

        public CombineView<SomeEntity, decimal> SumOfValuesView { get; }

        public SomeModel()
        {
            SumOfValuesView = new CombineView<SomeEntity, decimal>(SetEntities,
                (agg, item) => agg += item.Value,
                (agg, item) => agg -= item.Value);
        }
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
