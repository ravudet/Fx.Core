namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IDistinctByEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> DistinctBy<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.DistinctByDefault(keySelector);
        }

        public IV2Enumerable<TSource> DistinctBy<TKey>(Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return this.DistinctByDefault(keySelector, comparer);
        }
    }
}