namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IIntersectEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Intersect(IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            return this.IntersectDefault(second, comparer);
        }

        public IV2Enumerable<TSource> Intersect(IV2Enumerable<TSource> second)
        {
            return this.IntersectDefault(second);
        }
    }
}