﻿namespace System.Linq.V2
{
    public interface IAppendableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Append(TSource element)
        {
            return this.AppendDefault(element);
        }
    }
}