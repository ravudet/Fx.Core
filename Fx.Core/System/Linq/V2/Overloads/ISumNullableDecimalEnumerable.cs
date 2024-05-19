namespace System.Linq.V2
{
    public interface ISumNullableDecimalEnumerable : IV2Enumerable<decimal?>
    {
        public decimal? Sum()
        {
            return this.SumDefault();
        }
    }
}