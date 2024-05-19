namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IToHashSetEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public HashSet<TSource> ToHashSet()
        {
            return this.ToHashSetDefault();
        }

        public HashSet<TSource> ToHashSet(IEqualityComparer<TSource>? comparer)
        {
            return this.ToHashSetDefault(comparer);
        }
    }
}