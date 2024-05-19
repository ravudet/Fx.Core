namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IToDictionaryEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public Dictionary<TKey, TSource> ToDictionary<TKey>(Func<TSource, TKey> keySelector)
            where TKey : notnull
        {
            return this.ToDictionaryDefault(keySelector);
        }

        public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            return this.ToDictionaryDefault(keySelector, elementSelector, comparer);
        }

        public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
            where TKey : notnull
        {
            return this.ToDictionaryDefault(keySelector, elementSelector);
        }

        public Dictionary<TKey, TSource> ToDictionary<TKey>(
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            return this.ToDictionaryDefault(keySelector, comparer);
        }
    }
}