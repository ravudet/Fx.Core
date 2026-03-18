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

        [TestMethod]
        public void IsTrueNullable()
        {
            bool? condition = null;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));
            condition = true;
            Assert.That.IsTrue(condition);
            condition = false;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));
        }
    }
}
