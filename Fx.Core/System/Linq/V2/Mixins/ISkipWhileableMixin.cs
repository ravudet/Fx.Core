namespace System.Linq.V2
{
    public interface ISkipWhileableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> SkipWhile(Func<TSource, bool> predicate)
        {
            return this.SkipWhileDefault(predicate);
        }

        public IV2Enumerable<TSource> SkipWhile(Func<TSource, int, bool> predicate)
        {
            return this.SkipWhileDefault(predicate);
        }
    }
}