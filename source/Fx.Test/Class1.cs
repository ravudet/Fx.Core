namespace Fx.Test
{
    using System.Threading.Tasks;

    public static class Class1
    {
        public static ITask<int> DoWork()
        {
            return Task.FromResult(1).ToTaskWrapper();
        }
    }
}
