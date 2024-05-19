namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IOrderByEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2OrderedEnumerable<TSource> OrderBy<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return this.OrderByDefault(keySelector, comparer);
        }

        public IV2OrderedEnumerable<TSource> OrderBy<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.OrderByDefault(keySelector);
        }
    }
}