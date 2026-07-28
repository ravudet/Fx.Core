namespace System.Runtime.CompilerServices
{
    public interface IAwaitable<out T> : IAwaitable<IAwaiter<T>, T>
#if NET6_0_OR_GREATER
        where T : allows ref struct
#endif
    {
    }
}
