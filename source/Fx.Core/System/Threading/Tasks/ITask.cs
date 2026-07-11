namespace System.Threading.Tasks
{
    public interface ITask<out T> : IAwaitable<T>
        where T : allows ref struct
    {
        IAwaitable<T> ConfigureAwait(bool continueOnCapturedContext);
    }
}
