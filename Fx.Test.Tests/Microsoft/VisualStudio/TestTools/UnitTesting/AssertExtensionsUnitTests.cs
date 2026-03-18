namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {
        [TestMethod]
        public void IsTrue()
        {
            Assert.That.IsTrue(true);
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(false));
        }
    }
}
