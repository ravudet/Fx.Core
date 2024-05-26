namespace System.Linq.V2
{
    public interface IMaxableNullableDecimalMixin : IV2Enumerable<decimal?>
    {
        public decimal? Max()
        {
            return this.MaxDefault();
        }
    }
}