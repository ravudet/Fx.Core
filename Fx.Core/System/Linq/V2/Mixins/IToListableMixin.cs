﻿namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IToListableMixin<TSource> : IV2Enumerable<TSource>
    {
        public List<TSource> ToList()
        {
            return this.ToListDefault();
        }
    }
}