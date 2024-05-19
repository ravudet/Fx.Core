namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IExceptByEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> ExceptBy<TKey>(IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            return this.ExceptByDefault(second, keySelector);
        }

        public IV2Enumerable<TSource> ExceptBy<TKey>(
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return this.ExceptByDefault(second, keySelector, comparer);
        }
    }
}