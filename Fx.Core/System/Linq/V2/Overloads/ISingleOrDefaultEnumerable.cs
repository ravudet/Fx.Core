namespace System.Linq.V2
{
    public interface ISingleOrDefaultEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource SingleOrDefault(Func<TSource, bool> predicate, TSource defaultValue)
        {
            return this.SingleOrDefaultDefault(predicate, defaultValue);
        }

        public TSource? SingleOrDefault(Func<TSource, bool> predicate)
        {
            return this.SingleOrDefaultDefault(predicate);
        }

        public TSource? SingleOrDefault()
        {
            return this.SingleOrDefaultDefault();
        }

        public TSource SingleOrDefault(TSource defaultValue)
        {
            return this.SingleOrDefaultDefault(defaultValue);
        }
    }
}