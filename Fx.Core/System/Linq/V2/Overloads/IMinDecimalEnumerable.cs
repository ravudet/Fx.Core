namespace System.Linq.V2
{
    public interface IMinDecimalEnumerable : IV2Enumerable<decimal>
    {
        public decimal Min()
        {
            return this.MinDefault();
        }
    }
}