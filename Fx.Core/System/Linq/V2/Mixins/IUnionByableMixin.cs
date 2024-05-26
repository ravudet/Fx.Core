namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IUnionByableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> UnionBy<TKey>(IV2Enumerable<TSource> second, Func<TSource, TKey> keySelector)
        {
            return this.UnionByDefault(second, keySelector);
        }

        public IV2Enumerable<TSource> UnionBy<TKey>(
            IV2Enumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.UnionByDefault(second, keySelector, comparer);
        }
    }
}