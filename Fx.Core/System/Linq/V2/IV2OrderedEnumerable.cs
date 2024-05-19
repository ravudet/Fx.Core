namespace System.Linq.V2
{
    using System;
    using System.Collections.Generic;

    public interface IV2OrderedEnumerable<out TElement> : IV2Enumerable<TElement>, IV2Enumerable
    {
        IV2OrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending);
    }
}