namespace CombineDB.UnitTests
{
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
}
