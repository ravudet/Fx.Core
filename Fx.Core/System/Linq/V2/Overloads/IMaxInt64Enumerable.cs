namespace System.Linq.V2
{
    public interface IMaxInt64Enumerable : IV2Enumerable<long>
    {
        public long Max()
        {
            return this.MaxDefault();
        }
    }
}