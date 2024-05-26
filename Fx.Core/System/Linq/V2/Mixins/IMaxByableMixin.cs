namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IMaxByableMixin<TSource> : IV2Enumerable<TSource>
    {
        public TSource? MaxBy<TKey>(Func<TSource, TKey> keySelector)
        {
            return this.MaxByDefault(keySelector);
        }

        public TSource? MaxBy<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return this.MaxByDefault(keySelector, comparer);
        }
    }
}