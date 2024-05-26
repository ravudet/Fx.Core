namespace System.Linq.V2
{
    public interface IAverageableDecimalMixin : IV2Enumerable<decimal>
    {
        public decimal Average()
        {
            return this.AverageDefault();
        }
    }
}