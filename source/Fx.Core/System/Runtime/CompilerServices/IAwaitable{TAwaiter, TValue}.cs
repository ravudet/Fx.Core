namespace System.Runtime.CompilerServices
{
    public interface IAwaitable<out TAwaiter, out TValue>
        where TAwaiter : IAwaiter<TValue>
#if NET6_0_OR_GREATER
        , allows ref struct
#endif
#if NET6_0_OR_GREATER
        where TValue : allows ref struct
#endif
    {
        TAwaiter GetAwaiter();
    }
}
