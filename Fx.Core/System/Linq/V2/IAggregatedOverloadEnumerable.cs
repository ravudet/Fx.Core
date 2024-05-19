namespace System.Linq.V2
{
    public interface IAggregatedOverloadEnumerable<TElement> : IV2Enumerable<TElement>
    {
        IV2Enumerable<TElement> Source { get; }

        IAggregatedOverloadEnumerable<TSource> Create<TSource>(IV2Enumerable<TSource> source);
    }
}
