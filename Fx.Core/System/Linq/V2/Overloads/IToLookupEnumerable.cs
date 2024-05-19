namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IToLookupEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Lookup<TKey, TElement> ToLookup<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.ToLookupDefault(keySelector, elementSelector, comparer);
        }

        public IV2Lookup<TKey, TSource> ToLookup<TKey>(Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return this.ToLookupDefault(keySelector, comparer);
        }

        public IV2Lookup<TKey, TSource> ToLookup<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.ToLookupDefault(keySelector);
        }

        public IV2Lookup<TKey, TElement> ToLookup<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return this.ToLookupDefault(keySelector, elementSelector);
        }
    }
}