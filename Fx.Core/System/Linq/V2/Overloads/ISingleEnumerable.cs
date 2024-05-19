namespace System.Linq.V2
{
    public interface ISingleEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource Single()
        {
            return this.SingleDefault();
        }

        public TSource Single(Func<TSource, bool> predicate)
        {
            return this.SingleDefault(predicate);
        }
    }
}