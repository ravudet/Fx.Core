namespace System.Linq.V2
{
    public delegate IEnumerableMonad<TSource> Unit<TSource>(IV2Enumerable<TSource> source);

    public interface IEnumerableMonad<out TElement> : IV2Enumerable<TElement>
    {
        IV2Enumerable<TElement> Source { get; }

        Unit<TSource> Unit<TSource>();
    }
}
