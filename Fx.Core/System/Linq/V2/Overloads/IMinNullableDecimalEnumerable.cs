namespace System.Linq.V2
{
    public interface IMinNullableDecimalEnumerable : IV2Enumerable<decimal?>
    {
        public decimal? Min()
        {
            return this.MinDefault();
        }
    }
}