[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersion("1.0.0")]

namespace MethodWithTaskReturnType
{
    using System.Threading.Tasks;

    public static class Foo
    {
        public static Task<int> DoWork()
        {
            return Task.FromResult(1);
        }
    }
}