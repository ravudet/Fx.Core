namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IMaxEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public long Max(Func<TSource, long> selector)
        {
            return this.MaxDefault(selector);
        }

        public long? Max(Func<TSource, long?> selector)
        {
            return this.MaxDefault(selector);
        }

        public float? Max(Func<TSource, float?> selector)
        {
            return this.MaxDefault(selector);
        }

        public int? Max(Func<TSource, int?> selector)
        {
            return this.MaxDefault(selector);
        }

        public TSource? Max(IComparer<TSource>? comparer)
        {
            return this.MaxDefault(comparer);
        }

        public decimal? Max(Func<TSource, decimal?> selector)
        {
            return this.MaxDefault(selector);
        }

        public int Max(Func<TSource, int> selector)
        {
            return this.MaxDefault(selector);
        }

        public double Max(Func<TSource, double> selector)
        {
            return this.MaxDefault(selector);
        }

        public decimal Max(Func<TSource, decimal> selector)
        {
            return this.MaxDefault(selector);
        }

        public TResult? Max<TResult>(Func<TSource, TResult> selector)
        {
            return this.MaxDefault(selector);
        }

        public float Max(Func<TSource, float> selector)
        {
            return this.MaxDefault(selector);
        }

        public TSource? Max()
        {
            return this.MaxDefault();
        }

        public double? Max(Func<TSource, double?> selector)
        {
            return this.MaxDefault(selector);
        }
    }
}