using CombineDB.Core;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CombineDB.UnitTests
{
    [TestFixture]
    public class CombineContextTests
    {
        private ICombineContext<SomeModel> _context;

        [SetUp]
        public void SetUp()
        {
            _context = new CombineContext<SomeModel>();
        }

        [Test]
        public void CombineContext_Implements_ICombineContext()
        {
            Assert.True(typeof(CombineContext<SomeModel>).GetInterfaces().Contains(typeof(ICombineContext<SomeModel>)));
        }

        [Test]
        public async Task CombineContext_Executes_One_Command()
        {
            // arrange

            // act
            var result1 = await _context.ExecuteAsync(new SomeAddCommand(new SomeEntity(1, 100)));
            var result2 = await _context.ExecuteAsync(new SomeGetQuery(1));

            // assert
            Assert.AreSame(result1, result2);
        }
    }
}
