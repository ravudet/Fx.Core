namespace System.Linq.V2
{
    public static class EnumerableMonadExtensions
    {
        public static IEnumerableMonad<TUnit> Create<TElement, TUnit>(this IEnumerableMonad<TElement> monad, IV2Enumerable<TUnit> enumerable)
        {
            /*if (monad is IEnumerableMonad<TElement> nestedMonad)
            {
                enumerable = nestedMonad.Create(enumerable);
            }*/

            return monad.Unit<TUnit>()(enumerable);
        }
    }
}
