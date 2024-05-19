namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IMinByEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource? MinBy<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return this.MinByDefault(keySelector, comparer);
        }

        public TSource? MinBy<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.MinByDefault(keySelector);
        }
    }
}