namespace System.Linq.V2
{
    public interface IOfTypeEnumerable<TSource> : IV2Enumerable<TSource>
    {
        IV2Enumerable<TResult> OfType<TResult>(IV2Enumerable self); //// TODO
    }
}