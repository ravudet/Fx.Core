namespace System.Linq.V2
{
    public interface IAverageNullableDecimalEnumerable : IV2Enumerable<decimal?>
    {
        public decimal? Average()
        {
            return this.AverageDefault();
        }
    }
}