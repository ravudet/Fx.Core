namespace Playground
{
    public class Class1
    {
        public ITask<int> DoWork()
        {
            return Task.FromResult(1);
        }
    }
}
