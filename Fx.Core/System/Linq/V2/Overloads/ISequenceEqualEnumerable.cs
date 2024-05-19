namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface ISequenceEqualEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public bool SequenceEqual(IV2Enumerable<TSource> second)
        {
            return this.SequenceEqualDefault(second);
        }

        public bool SequenceEqual(IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            return this.SequenceEqualDefault(second, comparer);
        }
    }
}