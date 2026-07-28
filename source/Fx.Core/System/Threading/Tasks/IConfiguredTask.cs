namespace System.Threading.Tasks
{
    using System.Runtime.CompilerServices;

    public interface IConfiguredTask<out T> : IAwaitable<T>
#if NET6_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Exception? Exception { get; }

        bool IsCanceled { get; }
    }
}
