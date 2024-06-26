﻿namespace System.Linq.V2
{
    public interface IAnyableMixin<TSource> : IV2Enumerable<TSource>
    {
        public bool Any()
        {
            return this.AnyDefault();
        }

        public bool Any(Func<TSource, bool> predicate)
        {
            return this.AnyDefault(predicate);
        }
    }
}