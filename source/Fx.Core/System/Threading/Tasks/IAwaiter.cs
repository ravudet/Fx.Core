namespace System.Threading.Tasks
{
    using System.Runtime.CompilerServices;

    public interface IAwaiter<out T> : ICriticalNotifyCompletion
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        bool IsCompleted { get; }

        T GetResult();
    }
}
