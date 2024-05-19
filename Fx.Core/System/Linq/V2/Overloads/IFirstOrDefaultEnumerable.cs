namespace System.Linq.V2
{
    public interface IFirstOrDefaultEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource? FirstOrDefault()
        {
            return this.FirstOrDefaultDefault();
        }

        public TSource? FirstOrDefault(Func<TSource, bool> predicate)
        {
            return this.FirstOrDefaultDefault(predicate);
        }

        public TSource FirstOrDefault(Func<TSource, bool> predicate, TSource defaultValue)
        {
            return this.FirstOrDefaultDefault(predicate, defaultValue);
        }

        public TSource FirstOrDefault(TSource defaultValue)
        {
            return this.FirstOrDefaultDefault(defaultValue);
        }
    }
}