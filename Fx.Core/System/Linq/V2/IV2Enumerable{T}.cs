namespace System.Linq.V2
{
    using System.Collections.Generic;

    public interface IV2Enumerable<out T> : IEnumerable<T>, IV2Enumerable
    {
    }
}