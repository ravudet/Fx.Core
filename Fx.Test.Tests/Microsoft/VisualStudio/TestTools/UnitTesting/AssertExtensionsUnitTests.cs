namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System.Linq;

    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {
        //// TODO you are at identation preferences and you are re-ordering the config file to follow the ui; make sure there are no duplicates








        //// TODO i'm not convinced that the UI is a canonical list



        //// TODO add custom roslyn analayzer for `assert.that`





        public static void Foo()
        {
            var data = new[] { 1, 2, 3 };

            var doubled = from datum in data from datum2 in data 
                          select datum * datum2;
        }


        class Bar
        {
            public string? Fizz { get; set; }
            public string? Buzz { get; set; }
        }




















        [TestMethod]
        public void IsTrue()
        {
            bool condition = true;
            Assert.That.IsTrue(condition);

            condition = false;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));
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

        [TestMethod]
        public void IsTrueMessage()
        {
            var message = "some message";
            bool condition = true;
            Assert.That.IsTrue(condition, message);

            condition = false;
            var exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));
        }

        [TestMethod]
        public void IsTrueNullableMessage()
        {
            var message = "some message";
            bool? condition = null;
            var exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));

            condition = true;
            Assert.That.IsTrue(condition);

            condition = false;
            message = "another message";
            exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));
        }
    }
}
