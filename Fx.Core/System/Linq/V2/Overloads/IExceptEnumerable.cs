namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IExceptEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Except(IV2Enumerable<TSource> second)
        {
            return this.ExceptDefault(second);
        }

        public IV2Enumerable<TSource> Except(IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            return this.ExceptDefault(second, comparer);
        }
    }
}