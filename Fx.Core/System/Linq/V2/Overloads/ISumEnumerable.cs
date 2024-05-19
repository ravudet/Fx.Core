namespace System.Linq.V2
{
    public interface ISumEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public int Sum(Func<TSource, int> selector)
        {
            return this.SumDefault(selector);
        }

        public double? Sum(Func<TSource, double?> selector)
        {
            return this.SumDefault(selector);
        }

        public float Sum(Func<TSource, float> selector)
        {
            return this.SumDefault(selector);
        }

        public float? Sum(Func<TSource, float?> selector)
        {
            return this.SumDefault(selector);
        }

        public int? Sum(Func<TSource, int?> selector)
        {
            return this.SumDefault(selector);
        }

        public double Sum(Func<TSource, double> selector)
        {
            return this.SumDefault(selector);
        }

        public long? Sum(Func<TSource, long?> selector)
        {
            return this.SumDefault(selector);
        }

        public decimal? Sum(Func<TSource, decimal?> selector)
        {
            return this.SumDefault(selector);
        }

        public long Sum(Func<TSource, long> selector)
        {
            return this.SumDefault(selector);
        }

        public decimal Sum(Func<TSource, decimal> selector)
        {
            return this.SumDefault(selector);
        }
    }
}