namespace System.Linq.V2
{
    public interface ITakeLastEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> TakeLast(int count)
        {
            return this.TakeLastDefault(count);
        }
    }
}