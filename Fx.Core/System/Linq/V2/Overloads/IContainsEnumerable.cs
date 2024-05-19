namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IContainsEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public bool Contains(TSource value, IEqualityComparer<TSource>? comparer)
        {
            return this.ContainsDefault(value, comparer);
        }

        public bool Contains(TSource value)
        {
            return this.ContainsDefault(value);
        }
    }
}