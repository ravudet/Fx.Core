namespace System.Linq.V2
{
    public interface ISumableNullableInt64Mixin : IV2Enumerable<long?>
    {
        public long? Sum()
        {
            return this.SumDefault();
        }
    }
}