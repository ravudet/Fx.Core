namespace System.Runtime.CompilerServices
{
    public interface IConfiguredAwaitable<out T> : IConfigurableAwaitable<IAwaiter<T>, T, IAwaitable<T>, IAwaiter<T>>
#if NET6_0_OR_GREATER
        where T : allows ref struct
#endif
    {
    }
}
