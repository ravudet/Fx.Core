namespace System.Linq.V2
{
    public interface IMaxNullableDecimalEnumerable : IV2Enumerable<decimal?>
    {
        public decimal? Max()
        {
            return this.MaxDefault();
        }
    }
}