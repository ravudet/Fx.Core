namespace System.Linq.V2
{
    public interface IMaxDecimalEnumerable : IV2Enumerable<decimal>
    {
        public decimal Max()
        {
            return this.MaxDefault();
        }
    }
}