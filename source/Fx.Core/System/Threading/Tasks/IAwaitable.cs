namespace System.Threading.Tasks
{
    public interface IAwaitable<out T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        IAwaiter<T> GetAwaiter();
    }
}
