namespace System.Runtime.CompilerServices
{
    public interface IConfigurableAwaitable<out TDefaultAwaiter, out TValue, out TConfiguredAwaitable, out TConfiguredAwaiter> : IAwaitable<TDefaultAwaiter, TValue>
        where TDefaultAwaiter : IAwaiter<TValue>
#if NET6_0_OR_GREATER
        , allows ref struct
#endif
#if NET6_0_OR_GREATER
        where TValue : allows ref struct
#endif
        where TConfiguredAwaitable : IAwaitable<TConfiguredAwaiter, TValue>
#if NET6_0_OR_GREATER
        , allows ref struct
#endif
        where TConfiguredAwaiter : IAwaiter<TValue>
#if NET6_0_OR_GREATER
        , allows ref struct
#endif
    {
        TConfiguredAwaitable ConfigureAwait(bool continueOnCapturedContext); //// TODO use the enum and have an extension for the bool instead
    }
}
