namespace System.Linq.V2
{
    public interface IAverageEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public double Average(Func<TSource, int> selector)
        {
            return this.AverageDefault(selector);
        }

        public double? Average(Func<TSource, int?> selector)
        {
            return this.AverageDefault(selector);
        }

        public decimal Average(Func<TSource, decimal> selector)
        {
            return this.AverageDefault(selector);
        }

        public double Average(Func<TSource, double> selector)
        {
            return this.AverageDefault(selector);
        }

        public float? Average(Func<TSource, float?> selector)
        {
            return this.AverageDefault(selector);
        }

        public double? Average(Func<TSource, long?> selector)
        {
            return this.AverageDefault(selector);
        }

        public float Average(Func<TSource, float> selector)
        {
            return this.AverageDefault(selector);
        }

        public double? Average(Func<TSource, double?> selector)
        {
            return this.AverageDefault(selector);
        }

        public double Average(Func<TSource, long> selector)
        {
            return this.AverageDefault(selector);
        }

        public decimal? Average(Func<TSource, decimal?> selector)
        {
            return this.AverageDefault(selector);
        }
    }
}