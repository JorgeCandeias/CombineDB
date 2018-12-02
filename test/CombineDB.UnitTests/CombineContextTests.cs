using CombineDB.Core;
using NUnit.Framework;
using System.Linq;

namespace CombineDB.UnitTests
{
    [TestFixture]
    public class CombineContextTests
    {
        [Test]
        public void CombineContext_Implements_ICombineContext()
        {
            Assert.True(typeof(CombineContext).GetInterfaces().Contains(typeof(ICombineContext)));
        }
    }
}
