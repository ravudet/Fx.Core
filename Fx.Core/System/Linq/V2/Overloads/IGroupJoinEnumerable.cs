namespace System.Linq.V2
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public interface IGroupJoinEnumerable<TOuter> : IV2Enumerable<TOuter>
    {
        public IV2Enumerable<TResult> GroupJoin<TInner, TKey, TResult>(
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.GroupJoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public IV2Enumerable<TResult> GroupJoin<TInner, TKey, TResult>(
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector)
        {
            return this.GroupJoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector);
        }
    }
}