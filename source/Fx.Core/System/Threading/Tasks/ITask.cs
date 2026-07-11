namespace System.Threading.Tasks
{
    public interface ITask<out T>
        where T : allows ref struct
    {
    }
}
