namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IGroupByEnumerable<TSource> : IV2Enumerable<TSource> //// TODO use out parameters
    {
        public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.GroupByDefault(keySelector, elementSelector, resultSelector, comparer);
        }

        public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.GroupByDefault(keySelector, elementSelector, comparer);
        }

        public IV2Enumerable<IV2Grouping<TKey, TSource>> GroupBy<TKey>(
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.GroupByDefault(keySelector, comparer);
        }

        public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return this.GroupByDefault(keySelector, elementSelector);
        }

        public IV2Enumerable<IV2Grouping<TKey, TSource>> GroupBy<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.GroupByDefault(keySelector);
        }

        public IV2Enumerable<TResult> GroupBy<TKey, TResult>(
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector)
        {
            return this.GroupByDefault(keySelector, resultSelector);
        }

        public IV2Enumerable<TResult> GroupBy<TKey, TResult>(
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.GroupByDefault(keySelector, resultSelector, comparer);
        }

        public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector)
        {
            return this.GroupByDefault(keySelector, elementSelector, resultSelector);
        }
    }
}