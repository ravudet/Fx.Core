namespace System.Runtime.CompilerServices
{
    using System.Threading.Tasks;

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
        TConfiguredAwaitable ConfigureAwait(ConfigureAwaitOptions configureAwaitOptions); //// TODO have an extension method for the bool overload
    }
}
