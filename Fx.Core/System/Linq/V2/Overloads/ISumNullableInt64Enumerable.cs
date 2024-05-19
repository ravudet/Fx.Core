namespace System.Linq.V2
{
    public interface ISumNullableInt64Enumerable : IV2Enumerable<long?>
    {
        public long? Sum()
        {
            return this.SumDefault();
        }
    }
}