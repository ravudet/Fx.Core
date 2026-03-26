namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Linq;



    public class Buzz(int c)
    {
        public int C { get; } = c;
    }

    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {
        //// TODO you are re-ordering the config file to follow the ui; make sure there are no duplicates
        //// TODO what is making the "whitespace" rules warnings? in the other tabs, you can set the severity



        //// TODO `csharp_style_prefer_method_group_conversion = false:warning` flags `public static void Fizz(Action action) { } Fizz(() => Foo());` just the same as `csharp_style_prefer_method_group_conversion = true:warning` does





        //// TODO i'm not convinced that the UI is a canonical list



        //// TODO add custom roslyn analayzer for `assert.that`
        //// TODO add an analyzer that *disallows* primary constructors (the opposite of csharp_style_prefer_primary_constructors)





        public static void Foo()
        {
            var buzz = new Buzz(1234);


            var data = new[] { 1, 2, 3 };

            var doubled = from datum in data from datum2 in data 
                          select datum * datum2;

            Fizz(() => Foo());
        }

        public static void Fizz(Action action) { }


        class Bar
        {
            public string? Fizz { get; set; }
            public string? Buzz { get; set; }

            public string? GetOther() => this.Fizz;
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
