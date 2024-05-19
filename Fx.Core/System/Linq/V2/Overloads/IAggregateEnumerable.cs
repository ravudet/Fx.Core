namespace System.Linq.V2
{
    public interface IAggregateEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource Aggregate(Func<TSource, TSource, TSource> func)
        {
            return this.AggregateDefault(func);
        }

        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return this.AggregateDefault(seed, func);
        }

        public TResult Aggregate<TAccumulate, TResult>(
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            return this.AggregateDefault(seed, func, resultSelector);
        }
    }
}