namespace System.Runtime.CompilerServices
{
    public interface IAwaitable<out T>
#if NET6_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        IAwaiter<T> GetAwaiter();
    }
}
