namespace System.Linq.V2
{
    public interface IMinInt64Enumerable : IV2Enumerable<long>
    {
        public long Min()
        {
            return this.MinDefault();
        }
    }
}