namespace System.Linq.V2
{
    public interface ISumableNullableDecimalMixin : IV2Enumerable<decimal?>
    {
        public decimal? Sum()
        {
            return this.SumDefault();
        }
    }
}