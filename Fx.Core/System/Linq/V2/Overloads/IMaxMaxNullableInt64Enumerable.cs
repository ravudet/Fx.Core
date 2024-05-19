namespace System.Linq.V2
{
    public interface IMaxMaxNullableInt64Enumerable : IV2Enumerable<long?>
    {
        public long? Max()
        {
            return this.MaxDefault();
        }
    }
}