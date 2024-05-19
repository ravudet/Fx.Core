namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IIntersectByEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> IntersectBy<TKey>(IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            return this.IntersectByDefault(second, keySelector);
        }

        public IV2Enumerable<TSource> IntersectBy<TKey>(
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.IntersectByDefault(second, keySelector, comparer);
        }
    }
}