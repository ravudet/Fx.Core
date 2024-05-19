namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IJoinEnumerable<TOuter> : IV2Enumerable<TOuter>
    {
        public IV2Enumerable<TResult> Join<TInner, TKey, TResult>(
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return this.JoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public IV2Enumerable<TResult> Join<TInner, TKey, TResult>(
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.JoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }
    }
}