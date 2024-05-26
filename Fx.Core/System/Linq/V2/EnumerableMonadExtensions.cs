namespace System.Linq.V2
{
    public static class EnumerableMonadExtensions
    {
        public static IEnumerableMonad<TUnit> Create<TElement, TUnit>(this IEnumerableMonad<TElement> monad, IV2Enumerable<TUnit> enumerable)
        {
            return monad.Unit<TUnit>()(enumerable);
        }
    }
}
