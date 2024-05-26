namespace System.Linq.V2
{
    public interface IMaxableDecimalMixin : IV2Enumerable<decimal>
    {
        public decimal Max()
        {
            return this.MaxDefault();
        }
    }
}