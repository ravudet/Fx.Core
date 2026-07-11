namespace System.Threading.Tasks
{
    public interface ITask<out T> : IAwaitable<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        IAwaitable<T> ConfigureAwait(bool continueOnCapturedContext);
    }
}
