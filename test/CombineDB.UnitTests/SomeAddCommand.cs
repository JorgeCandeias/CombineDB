using System.Threading.Tasks;
using CombineDB.Core;

namespace CombineDB.UnitTests
{
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
}
