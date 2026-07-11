namespace System.Threading.Tasks
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class TaskExtensionsUnitTests
    {
        [TestMethod]
        public async Task ToTaskWrapper()
        {
            var expectedValue = 42;
            var value = await TaskExtensionsUnitTests.ToTaskWrapperHelper(expectedValue);

            Assert.AreEqual(expectedValue, value);
        }

        private static TaskWrapper<int> ToTaskWrapperHelper(int value)
        {
            return ToTaskWrapperHelperImpl(value).ToTaskWrapper();
        }

        private static async Task<int> ToTaskWrapperHelperImpl(int value)
        {
            return await Task.FromResult(value);
        }
    }
}
