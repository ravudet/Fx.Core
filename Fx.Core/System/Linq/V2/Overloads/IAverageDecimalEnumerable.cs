namespace System.Linq.V2
{
    public interface IAverageDecimalEnumerable : IV2Enumerable<decimal>
    {
        public decimal Average()
        {
            return this.AverageDefault();
        }
    }
}