namespace System.Linq.V2
{
    public interface ISumableInt64Mixin : IV2Enumerable<long>
    {
        public long Sum()
        {
            return this.SumDefault();
        }
    }
}