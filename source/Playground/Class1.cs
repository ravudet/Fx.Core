namespace Playground
{
    public class Class1
    {
        public async ITask<int> DoWork()
        {
            return await Fx.Test.Class1.DoWork();

            ////return Task.FromResult(1);
        }
    }
}
