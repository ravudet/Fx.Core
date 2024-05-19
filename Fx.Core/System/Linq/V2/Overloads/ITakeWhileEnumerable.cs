namespace System.Linq.V2
{
    public interface ITakeWhileEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> TakeWhile(Func<TSource, bool> predicate)
        {
            return this.TakeWhileDefault(predicate);
        }

        public IV2Enumerable<TSource> TakeWhile(Func<TSource, int, bool> predicate)
        {
            return this.TakeWhileDefault(predicate);
        }
    }
}