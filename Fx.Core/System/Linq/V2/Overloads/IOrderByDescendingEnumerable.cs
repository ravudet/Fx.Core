namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IOrderByDescendingEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2OrderedEnumerable<TSource> OrderByDescending<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.OrderByDescendingDefault(keySelector);
        }

        public IV2OrderedEnumerable<TSource> OrderByDescending<TKey>(
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            return this.OrderByDescendingDefault(keySelector, comparer);
        }
    }
}