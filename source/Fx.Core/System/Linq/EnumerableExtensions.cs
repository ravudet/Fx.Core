namespace System.Linq
{
    using System.Collections.Generic;
#if NETCOREAPP3_0_OR_GREATER
    using System.Diagnostics.CodeAnalysis;
#endif

    public static class EnumerableExtensions
    {
        public static bool TryFirst<TElement>(
            this IEnumerable<TElement> source,
#if NETCOREAPP3_0_OR_GREATER
            [MaybeNullWhen(false)]
#endif
            out TElement first)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    first = default(TElement)
#if !NETCOREAPP3_0_OR_GREATER
                        !
#endif
                        ;
                    return false;
                }

                first = enumerator.Current;
                return true;
            }
        }
    }
}
