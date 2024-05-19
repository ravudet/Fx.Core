namespace System.Linq.V2
{
    public interface ISumInt64Enumerable : IV2Enumerable<long>
    {
        public long Sum()
        {
            return this.SumDefault();
        }
    }
}