namespace System.Linq.V2
{
    public interface IAverageableNullableDecimalMixin : IV2Enumerable<decimal?>
    {
        public decimal? Average()
        {
            return this.AverageDefault();
        }
    }
}