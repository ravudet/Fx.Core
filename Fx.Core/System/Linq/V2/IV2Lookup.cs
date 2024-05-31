namespace System.Linq.V2
{
    public interface IV2Lookup<TKey, TElement> : IV2Enumerable<IV2Grouping<TKey, TElement>>, IV2Enumerable
    {
        IV2Enumerable<TElement> this[TKey key] { get; }

        int Count { get; }

        bool Contains(TKey key);
    }
}