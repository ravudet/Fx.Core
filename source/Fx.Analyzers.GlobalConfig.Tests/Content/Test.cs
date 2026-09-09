[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersion("1.0.0")]

namespace Test
{
    using System;
    using System.Collections.Generic;

    public static class Foo
    {
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            foreach (var element in enumerable)
            {
                yield return element;
            }
        }
    }
}