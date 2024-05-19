namespace System.Linq.V2
{
    public interface IWhereEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Where(Func<TSource, bool> predicate)
        {
            return this.WhereDefault(predicate);
        }

        public IV2Enumerable<TSource> Where(Func<TSource, int, bool> predicate)
        {
            return this.WhereDefault(predicate);
        }
    }
}