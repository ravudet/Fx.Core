namespace System.Linq.V2
{
    public interface ISumableDecimalMixin : IV2Enumerable<decimal>
    {
        public decimal Sum()
        {
            return this.SumDefault();
        }
    }
}