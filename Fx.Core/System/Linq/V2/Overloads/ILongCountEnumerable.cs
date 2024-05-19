namespace System.Linq.V2
{
    public interface ILongCountEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public long LongCount(Func<TSource, bool> predicate)
        {
            return this.LongCountDefault(predicate);
        }

        public long LongCount()
        {
            return this.LongCountDefault();
        }
    }
}