namespace System.Runtime.CompilerServices
{
    public interface IConfigurableAwaitable<out TDefaultAwaiter, out TValue, out TConfiguredAwaitable, out TConfiguredAwaiter> : IAwaitable<TDefaultAwaiter, TValue>
        where TDefaultAwaiter : IAwaiter<TValue>
#if NET6_0_OR_GREATER
        where TConfiguredAwaitable : IAwaitable<TConfiguredAwaiter, TValue>
        where TConfiguredAwaiter : IAwaiter<TValue>
    {
        TConfiguredAwaitable ConfigureAwait(bool continueOnCapturedContext); //// TODO use the enum and have an extension for the bool instead
    }
}
