namespace System.Linq.V2
{
    public interface ICastEnumerable : IV2Enumerable
    {
        //// TODO
        IV2Enumerable<TResult> Cast<TResult>();
    }
}