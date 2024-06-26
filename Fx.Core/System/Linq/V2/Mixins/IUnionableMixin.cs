﻿namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IUnionableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Union(IV2Enumerable<TSource> second)
        {
            return this.UnionDefault(second);
        }

        public IV2Enumerable<TSource> Union(IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            return this.UnionDefault(second, comparer);
        }
    }
}