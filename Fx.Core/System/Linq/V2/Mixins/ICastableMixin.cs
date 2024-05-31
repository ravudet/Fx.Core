namespace System.Linq.V2
{
    public interface ICastableMixin : IV2Enumerable
    {
        IV2Enumerable<TResult> Cast<TResult>();
    }
}