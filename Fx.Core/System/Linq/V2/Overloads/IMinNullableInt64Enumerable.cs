namespace System.Linq.V2
{
    public interface IMinNullableInt64Enumerable : IV2Enumerable<long?>
    {
        public long? Min()
        {
            return this.MinDefault();
        }
    }
}