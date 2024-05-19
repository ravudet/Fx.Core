namespace System.Linq.V2
{
    public interface ISkipLastEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> SkipLast(int count)
        {
            return this.SkipLastDefault(count);
        }
    }
}