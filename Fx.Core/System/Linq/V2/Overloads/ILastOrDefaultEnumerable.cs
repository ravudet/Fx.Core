namespace System.Linq.V2
{
    public interface ILastOrDefaultEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource? LastOrDefault()
        {
            return this.LastOrDefaultDefault();
        }

        public TSource LastOrDefault(Func<TSource, bool> predicate, TSource defaultValue)
        {
            return this.LastOrDefaultDefault(predicate, defaultValue);
        }

        public TSource? LastOrDefault(Func<TSource, bool> predicate)
        {
            return this.LastOrDefaultDefault(predicate);
        }

        public TSource LastOrDefault(TSource defaultValue)
        {
            return this.LastOrDefaultDefault(defaultValue);
        }
    }
}