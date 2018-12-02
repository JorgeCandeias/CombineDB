using System.Collections.Generic;

namespace CombineDB.UnitTests
{
    public class SomeModel
    {
        public Dictionary<int, SomeEntity> Entities = new Dictionary<int, SomeEntity>();

        public double SumOfValues { get; set; }
    }
}
