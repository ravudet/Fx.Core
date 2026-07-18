namespace Playground
{
    public class Class1
    {
        public ITask<int> DoWork()
        {
            ////return await Fx.Test.Class1.DoWork();

            return Task.FromResult(1).ToTaskWrapper();
        }
    }
}
