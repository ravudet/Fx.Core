namespace System.Threading.Tasks
{
    public interface IAwaitable<out T>
        where T : allows ref struct
    {
        IAwaiter<T> GetAwaiter();
    }
}
