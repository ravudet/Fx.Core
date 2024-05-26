namespace System.Linq.V2
{
    public interface ISumableInt32Mixin : IV2Enumerable<int>
    {
        public int Sum()
        {
            return this.SumDefault();
        }
    }
}