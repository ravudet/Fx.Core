namespace System.Linq.V2
{
    public interface IV2Grouping<out TKey, out TElement> : IV2Enumerable<TElement>, IV2Enumerable //// TODO do you need this type?
    {
        TKey Key { get; }
    }
}