namespace System.Linq.V2
{
    public interface IV2Grouping<out TKey, out TElement> : IV2Enumerable<TElement>, IV2Enumerable
    {
        TKey Key { get; }
    }
}