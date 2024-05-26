namespace System.Linq.V2
{
    public interface IMinableNullableDecimalMixin : IV2Enumerable<decimal?>
    {
        public decimal? Min()
        {
            return this.MinDefault();
        }
    }
}