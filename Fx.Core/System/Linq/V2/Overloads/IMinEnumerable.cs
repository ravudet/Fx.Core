namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IMinEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public int Min(Func<TSource, int> selector)
        {
            return this.MinDefault(selector);
        }

        public long Min(Func<TSource, long> selector)
        {
            return this.MinDefault(selector);
        }

        public decimal? Min(Func<TSource, decimal?> selector)
        {
            return this.MinDefault(selector);
        }

        public double? Min(Func<TSource, double?> selector)
        {
            return this.MinDefault(selector);
        }

        public int? Min(Func<TSource, int?> selector)
        {
            return this.MinDefault(selector);
        }

        public float? Min(Func<TSource, float?> selector)
        {
            return this.MinDefault(selector);
        }

        public float Min(Func<TSource, float> selector)
        {
            return this.MinDefault(selector);
        }

        public TResult? Min<TResult>(Func<TSource, TResult> selector)
        {
            return this.MinDefault(selector);
        }

        public double Min(Func<TSource, double> selector)
        {
            return this.MinDefault(selector);
        }

        public long? Min(Func<TSource, long?> selector)
        {
            return this.MinDefault(selector);
        }

        public TSource? Min()
        {
            return this.MinDefault();
        }

        public TSource? Min(IComparer<TSource>? comparer)
        {
            return this.MinDefault(comparer);
        }

        public decimal Min(Func<TSource, decimal> selector)
        {
            return this.MinDefault(selector);
        }
    }
}