using CombineDB.Core;
using System.Threading.Tasks;

namespace CombineDB.UnitTests
{
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
}
