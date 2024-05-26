namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IDistinctableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Distinct()
        {
            return this.DistinctDefault();
        }

        public IV2Enumerable<TSource> Distinct(IEqualityComparer<TSource>? comparer)
        {
            return this.DistinctDefault(comparer);
        }
    }
}