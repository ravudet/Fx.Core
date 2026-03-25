namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;

    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {
        //// TODO you are at identation preferences and you are re-ordering the config file to follow the ui; make sure there are no duplicates

        //// TODO i'm not sure i'm going to like `csharp_new_line_before_open_brace = all`; things like anonymous types and object/array initializers are sometimes small enough that inline works better, and having to use a suppression to get inline will ruin the look
        //// TODO csharp_new_line_before_members_in_object_initializers, same reasons as above
        //// TODO csharp_new_line_before_members_in_anonymous_types, same reasons as above







        //// TODO i'm not convinced that the UI is a canonical list



        //// TODO add custom roslyn analayzer for `assert.that`






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
            ////AssertExtensionsUnitTests foo = new AssertExtensionsUnitTests();
            ////var x = 0;

            Console.WriteLine();
            Console.WriteLine();

            const int x = 0;
            Console.WriteLine(x);

            var y = 0;
            Console.WriteLine(y);

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
