namespace System.Linq.V2
{
    public interface ILastEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource Last()
        {
            return this.LastDefault();
        }

        public TSource Last(Func<TSource, bool> predicate)
        {
            return this.LastDefault(predicate);
        }
    }
}