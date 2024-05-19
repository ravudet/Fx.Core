namespace System.Linq.V2
{
    public interface ISumDecimalEnumerable : IV2Enumerable<decimal>
    {
        public decimal Sum()
        {
            return this.SumDefault();
        }
    }
}