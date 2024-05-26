namespace System.Linq.V2
{
    public interface IMinableDecimalMixin : IV2Enumerable<decimal>
    {
        public decimal Min()
        {
            return this.MinDefault();
        }
    }
}