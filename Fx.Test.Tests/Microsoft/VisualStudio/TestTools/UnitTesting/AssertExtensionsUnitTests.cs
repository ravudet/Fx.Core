namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;

    public class Buzz(int c)
    {
        public int C { get; } = c;
    }

    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {
        //// TODO dotnet_diagnostic.CA1000.severity = warning doesn't flag the method call like the doc says
















        //// TODO you are re-ordering the config file to follow the ui; make sure there are no duplicates
        //// TODO what is making the "whitespace" rules warnings? in the other tabs, you can set the severity
        //// TODO go back through and determine which ones are "disabled" when `false` (rather than warning for the opposite); for these, decide on what "severity" you want to set (can you remove the severity altogether?)
        ////    TODO `csharp_style_prefer_method_group_conversion = false:warning` flags `public static void Fizz(Action action) { } Fizz(() => Foo());` does the same as `csharp_style_prefer_method_group_conversion = true:warning` does
        ////    TODO `dotnet_style_prefer_conditional_expression_over_assignment = false:warning` doesn't warn for `string something = true ? "asdf" : "qwer";`
        ////    TODO `dotnet_style_prefer_compound_assignment = false:warning` doesn't warn for `var value = 1324; value += 5;`
        ////    TODO `csharp_style_throw_expression = false:warning` doesn't warn for `var assigned = another ?? throw new Exception("TODO");`
        ////    TODO `csharp_style_prefer_index_operator = false:warning` doesn't warn for `var index = data[^1];`
        //// TODO for each rule, toggling back and forth should result in no change to the file (except the rules that you deleted)
        //// TODO go back through the rules again to see which ones you deleted; just comment them out instead






        //// TODO i can't get csharp_style_unused_value_assignment_preference to trigger, and it looks like CS0219 covers it anyway?
        //// TODO https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0042 seems to indicate `var person = GetPersonTuple();` is illegal, but it doesn't seem flagged to me; *i* prefer this, because i only want `(int x, int y) point = GetPointTuple();` to be fixed; this is either a bug or a doc issue, though




        //// TODO i'm not convinced that the UI is a canonical list; go through the rule reference documentation to see if there are any missing: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories



        //// TODO dotnet_style_namespace_match_folder is really good for me, but the article https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0130 mentions it's not going to work for command line builds without additional csproj maniupulation... (though it will work for vs builds)


        //// TODO add custom roslyn analayzer for `assert.that`
        ////    TODO allow unsafe is an example of a codefix that updates the project
        //// TODO add an analyzer that *disallows* primary constructors (the opposite of csharp_style_prefer_primary_constructors)
        //// TODO you know, the old style cop docs used to actually have pretty good justifications for why you want to change your code...
        //// TODO write articles to justify the non-style rules //// TODO do you want articles, or do you want to put them as comments in the config file? //// TODO probably write articles and link to them in the rules
        ////    TODO dotnet_style_explicit_tuple_names is really saying "*when* explicit names are available, use them", not "always add explicit names"
        ////    TODO dotnet_style_prefer_inferred_tuple_names is saying that, if a name is not changing, don't be explicit about this; i don't think i like any way that this rule can be configured, i would prefer that it's always either all inferred or all explicit; i'm disabling this rule and i'm going to write my own i think
        ////    TODO dotnet_style_prefer_inferred_anonymous_type_member_names is the same as inferred tuple names, and i have the same thoughts on it
        ////    TODO csharp_style_prefer_implicitly_typed_lambda_expression also triggers cases where you are passing a lambda as a parameter to a method `Frub((int x) => { });`
        ////    TODO for `csharp_style_prefer_tuple_swap` i wonder if the compiler or jiter do magic here; isn't the tuple going to take extra (stack) memory? i do think it looks neat though
        ////    TODO i think `csharp_style_inlined_variable_declaration` would make a lot more sense if `while (!int.TryParse(value, out var parsed)) { } Console.WriteLine(parsed);` worked




        public class Container<T>
        {
            public static void DoWork()
            {
            }

            public List<T> List { get; }
        }

        public class AnotherContainer
        {
            public void DoWork<T>()
            {
            }
        }


        public interface ITest
        {
            void Test();
        }

        public record Point(int X, int Y);
        public record Segment(Point Start, Point End);

        // Violates IDE0170.
        static bool IsEndOnXAxis(Segment segment) =>
            segment is { Start: { Y: 0, X: 5 } } or { End: { Y: 0 } };

        // Fixed code.
        static bool IsEndOnXAxis2(Segment segment) =>
            segment is { Start.Y: 0, Start.X: 5 } or { End.Y: 0 };


        public static void Frub(Action<int> action)
        {
            new AnotherContainer().DoWork<int>();
        }

        public static void Frob(string? thing, object bar)
        {
            Frub((int x) => { });

            Bar fizz = new("asdf");

            var derived = bar as Derived;

            if (derived is not Derived)
            {
            }

            ////var cValue = derived.Buzz.C;


            var sum = GetValue() * 3 + 2;

            if (sum is default(int) or 5)
            {
            }


            var other = thing != null ? thing : "asdf";
            other = thing == null ? "Asdf" : thing;

            if (thing == null)
            {
                throw new Exception("TODO");
            }

            var another = thing as object;
            var assigned = another ?? throw new Exception("TODO");

            if (another == null)
            {
                throw new Exception("tODO");
            }


            if (assigned == null) throw new Exception("tODO");


            string something = true ? "asdf" : "qwer";
            Console.WriteLine(something);


            var customer = GetTuple();
            Console.WriteLine(customer.Item1);

            (string name, int age) anotherCustomer = GetTuple();

            var more = (name2: customer.name, customer.age);

            Console.WriteLine(customer.ToString());
            Console.WriteLine($"{customer.ToString()}");

            Exception? subsequent = default;

            var tuple = GetTuple();
            var notTuple = AnotherParse("asdf");

            AnotherParse("asdf");

            Console.WriteLine("asdF");
        }

        public static int GetValue()
        {
            return 5;
        }

        public static (string name, int age) GetTuple()
        {
            return ("ASdf", 1234);
        }

        public static bool CustomParse(string toParse, [NotNullWhen(true)] out int? parsed)
        {
            var value = int.TryParse(toParse, out var intermediate);
            parsed = intermediate;
            return value;
        }

        public struct Structure
        {
        }

        public static Structure AnotherParse(string toParse)
        {
            return new Structure();
        }

        public static void Foo2()
        {
            var wasParsed = AnotherParse("asdf");

            while (!CustomParse("asdf", out var parsed))
            {
                parsed = 5;
            }

            ////Console.WriteLine(parsed);

            byte[] bytes = "ABC"u8.ToArray();

            var buzz = new Buzz(1234);

            var value = 1234;
            value += 5;

            var something = new { C2 = buzz.C, value2 = value };


            var data = new[] { 1, 2, 3 };
            var index = data[^1];
            var more = data[1..4];

            var doubled = from datum in data from datum2 in data 
                          select datum * datum2;

            Fizz(() => Foo2());

            var result = data.Select(
                foo
                    => foo * 2);
        }

        public static void Fizz(Action action) { }


        public class Bar
        {
            public string? Fizz { get; set; }

            public Bar(string? fizz) =>
                Fizz = fizz;

            public string? GetOther() => this.Fizz;
        }

        public class Derived :
            Bar,
            ISomething
        {
            public Derived(string? fizz)
                : base(
                      fizz)
            {
            }

            public void DoWork(int parameter)
            {
            }

            public Buzz Buzz { get; set; }
        }

        public interface ISomething
        {
            void DoWork(int parameter);
        }




        [TestMethod]
        public void Equality()
        {
            Console.WriteLine(nameof(System.Collections.Generic.List<string>));

            var first = new EqualsThing();

            Assert.IsTrue(first == null);
            Assert.IsFalse(object.ReferenceEquals(first, null));

            var second = new EqualsThing();

            Assert.IsFalse(object.ReferenceEquals(first, second));
            Assert.IsTrue(object.ReferenceEquals(first, first));

            var third = new EqualsStruct();
            Assert.IsFalse(object.ReferenceEquals(third, null));

            /*if (third is null)
            {
            }*/
        }

        // TODO: i think this plus the struct thing (plus maybe something with generics) demonstrate the differences if you look at the decompiled code
        public void M(Foo foo)
        {
            if (foo is null)
            {
            }

            if (object.ReferenceEquals(foo, null))
            {
            }

            if (foo == null)
            {
            }
        }

        public class Foo
        {
            public static bool operator ==(Foo? first, Foo? second)
            {
                return true;
            }

            public static bool operator !=(Foo? first, Foo? second)
            {
                return false;
            }
        }
        // ENDTODO

        public void Equality2<T>(T something)
        {
            // notice, this won't be noticed by CA2013 if t is a struct
            Assert.IsTrue(object.ReferenceEquals(something, null));

            if (something is null) // TODO unclear if this is just as bad as `object.referenceequals`
            {
            }
        }

        public struct EqualsStruct
        {
        }

        public class EqualsThing
        {
            public static bool operator ==(EqualsThing? left, EqualsThing? right)
            {
                return true;
            }

            public static bool operator !=(EqualsThing left, EqualsThing right)
            {
                return false;
            }
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
