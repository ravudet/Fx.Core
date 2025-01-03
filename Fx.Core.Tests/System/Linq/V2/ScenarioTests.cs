namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Numerics;
    using System.Reflection.Metadata.Ecma335;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void AverageDecimalMixinWithOverload()
        {

        }

        private sealed class MockAverageDecimalMixinWithOverload : IAverageableDecimalMixin
        {
            public static decimal Result { get; } = new object().GetHashCode();

            public decimal Average()
            {
                return Result;
            }

            public IEnumerator<decimal> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }

    public interface IEnumerableFactories
    {
        static abstract IV2Enumerable<T> Empty<T>();

        static abstract IV2Enumerable<T> Repeat<T>(T element, int count);
    }

    public sealed class RavudetExtension<TElement> : IEnumerableFactories, IEnumerableMonad<TElement>
    {
        public RavudetExtension(IV2Enumerable<TElement> source)
        {
            this.Source = source;
        }

        public IV2Enumerable<TElement> Source { get; }

        public static IV2Enumerable<T> Empty<T>()
        {
            throw new NotImplementedException();
        }

        public static IV2Enumerable<T> Repeat<T>(T element, int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Unit<TSource> Unit<TSource>()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public static class SpikeExtensions
    {
        public static IV2Enumerable<TElement> Driver<TEnumerable, TElement>(this TEnumerable source) where TEnumerable : IV2Enumerable<TElement>, IEnumerableFactories
        {
            if (!source.Take(5).Any())
            {
                return TEnumerable.Empty<TElement>();
            }
            else
            {
                return TEnumerable.Repeat(source.First(), 10);
            }
        }
    }

    public static class SomeExtensionsExtensions
    {
        public static IEnumerableMonad<T> ToSomeExtensions<T>(this IV2Enumerable<T> enumerable)
        {
            if (typeof(decimal) == typeof(T))
            {
                return (IEnumerableMonad<T>)new AverageDecimalMixin((IV2Enumerable<decimal>)enumerable).AsV2Enumerable();
            }
            else
            {
                return new SomeExtensions<T>(enumerable);
            }
        }

        /// <summary>
        /// these are some extensions that include, among other things (though left out of this implementation because i was lazy), the ability to compute the average of sequence of decimals in some optimal way
        /// 
        /// this means that if we end up going from `t` -> `decimal` at some point (like a select), we should be able to average in teh optimal way
        /// 
        /// it also means that if we end up going from `t` -> `decimal` -> `string` -> `decimal`, we should still be able to avarege in teh optimal way 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class SomeExtensions<T> : IEnumerableMonad<T>
        {
            public SomeExtensions(IV2Enumerable<T> source)
            {
                // it is imperative that this is only called by `tosomeextensions` because otherwise we have no way to overload the average of decimal functionality
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            public Unit<TSource> Unit<TSource>()
            {
                return ToSomeExtensions;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }

        private sealed class AverageDecimalMixin : IAverageableDecimalMixin, IEnumerableMonad<decimal>
        {
            public AverageDecimalMixin(IV2Enumerable<decimal> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<decimal> Source { get; }

            public IEnumerator<decimal> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            public Unit<TSource> Unit<TSource>()
            {
                return ToSomeExtensions;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }

            public decimal Average()
            {
                // no wonder it's so fast!
                return 0;
            }
        }
    }
    
    [TestClass]
    public sealed class ScenarioTests
    {
        private static void GenerateTyped(
            string operation,
            string overload,
            string overloadReturnType,
            string overloadTypeParameters,
            string overloadParameters,
            string elementType,
            string arguments,
            string sourceElementCount)
        {
            var template = System.IO.File.ReadAllText(@"C:\github\Fx.Core\TypedTemplate.txt");
            var escapedTemplate = template
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("{{0}}", "{0}")
                .Replace("{{1}}", "{1}")
                .Replace("{{2}}", "{2}")
                .Replace("{{3}}", "{3}")
                .Replace("{{4}}", "{4}")
                .Replace("{{5}}", "{5}")
                .Replace("{{6}}", "{6}")
                .Replace("{{7}}", "{7}")
                .Replace("{{6}}", string.Empty)
                .Replace("{{7}}", string.Empty)
                .Replace("{{8}}", "{8}")
                ;

            var generated = string.Format(
                escapedTemplate,
                operation,
                overload,
                overloadReturnType,
                overloadTypeParameters,
                overloadParameters,
                elementType,
                arguments,
                sourceElementCount,
                operation.ToLower()
                );

            var unescapedGenerated = generated
                .Replace("{{", "{")
                .Replace("}}", "}");
            System.IO.File.WriteAllText($@"C:\github\Fx.Core\Fx.Core.Tests\System\Linq\V2\V2EnumerableUnitTests_{operation}{overload}.cs", unescapedGenerated);
        }

        private static void GenerateTyped()
        {
            GenerateTyped(
                operation: "Average",
                overload: "Decimal",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "Double",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "Int32",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "Int64",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "NullableDecimal",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "NullableDouble",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "NullableInt32",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "NullableInt64",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "NullableSingle",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Average",
                overload: "Single",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float",
                arguments: string.Empty,
                sourceElementCount: "1");

            GenerateTyped(
                operation: "Max",
                overload: "Decimal",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "Double",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "Int32",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "Int64",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "NullableDecimal",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "NullableDouble",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "NullableInt32",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "NullableInt64",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "NullableSingle",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Max",
                overload: "Single",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float",
                arguments: string.Empty,
                sourceElementCount: "1");

            GenerateTyped(
                operation: "Min",
                overload: "Decimal",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "Double",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "Int32",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "Int64",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "NullableDecimal",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "Decimal",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "NullableDouble",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "NullableInt32",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "NullableInt64",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "NullableSingle",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Min",
                overload: "Single",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float",
                arguments: string.Empty,
                sourceElementCount: "1");

            GenerateTyped(
                operation: "Sum",
                overload: "Decimal",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "Double",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "Int32",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "Int64",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "NullableDecimal",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "decimal?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "NullableDouble",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "double?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "NullableInt32",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "int?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "NullableInt64",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "long?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "NullableSingle",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float?",
                arguments: string.Empty,
                sourceElementCount: "1");
            GenerateTyped(
                operation: "Sum",
                overload: "Single",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "",
                elementType: "float",
                arguments: string.Empty,
                sourceElementCount: "1");
        }

        private static void GenerateComplexTerminal(
            string operation,
            string overload,
            string overloadReturnType,
            string overloadTypeParameters,
            string overloadParameters,
            string resultType,
            string arguments,
            string defaultResult,
            string customResult)
        {
            var template = System.IO.File.ReadAllText(@"C:\github\Fx.Core\ComplexTerminalTemplate.txt");
            var escapedTemplate = template
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("{{0}}", "{0}")
                .Replace("{{1}}", "{1}")
                .Replace("{{2}}", "{2}")
                .Replace("{{3}}", "{3}")
                .Replace("{{4}}", "{4}")
                .Replace("{{5}}", "{5}")
                .Replace("{{6}}", "{6}")
                .Replace("{{7}}", "{7}")
                .Replace("{{8}}", "{8}")
                .Replace("{{9}}", "{9}")
                ;

            var generated = string.Format(
                escapedTemplate,
                operation,
                overload,
                overloadReturnType,
                overloadTypeParameters,
                overloadParameters,
                resultType,
                arguments,
                defaultResult,
                customResult,
                operation.ToLower()
                );

            var unescapedGenerated = generated
                .Replace("{{", "{")
                .Replace("}}", "}");
            System.IO.File.WriteAllText($@"C:\github\Fx.Core\Fx.Core.Tests\System\Linq\V2\V2EnumerableUnitTests_{overload}.cs", unescapedGenerated);
        }

        private static void GenerateComplexTerminal()
        {
            GenerateComplexTerminal(
                operation: "ToArray",
                overload: "ToArray",
                overloadReturnType: "object[]",
                overloadTypeParameters: string.Empty,
                overloadParameters: string.Empty,
                resultType: "object[]",
                arguments: string.Empty,
                defaultResult: "new object[0]",
                customResult: "new[] { new object(), new object() }"
                );

            GenerateComplexTerminal(
                operation: "ToHashSet",
                overload: "ToHashSet",
                overloadReturnType: "HashSet<object>",
                overloadTypeParameters: string.Empty,
                overloadParameters: string.Empty,
                resultType: "HashSet<object>",
                arguments: string.Empty,
                defaultResult: "new HashSet<object>()",
                customResult: "new HashSet<object>(new[] { new object(), new object() })"
                );
            GenerateComplexTerminal(
                operation: "ToHashSet",
                overload: "ToHashSetWithComparer",
                overloadReturnType: "HashSet<object>",
                overloadTypeParameters: string.Empty,
                overloadParameters: "IEqualityComparer<object>? comparer",
                resultType: "HashSet<object>",
                arguments: "EqualityComparer<object>.Default",
                defaultResult: "new HashSet<object>()",
                customResult: "new HashSet<object>(new[] { new object(), new object() })"
                );

            GenerateComplexTerminal(
                operation: "ToList",
                overload: "ToList",
                overloadReturnType: "List<object>",
                overloadTypeParameters: string.Empty,
                overloadParameters: string.Empty,
                resultType: "List<object>",
                arguments: string.Empty,
                defaultResult: "new List<object>()",
                customResult: "new List<object>(new[] { new object(), new object() })"
                );
        }

        private static void GenerateComplexMultipleTypesTerminal(
            string operation,
            string overload,
            string overloadReturnType,
            string overloadTypeParameters,
            string overloadParameters,
            string overloadGenericTypeConstraints,
            string resultType,
            string resultCast,
            string arguments,
            string defaultResult,
            string customResult)
        {
            var template = System.IO.File.ReadAllText(@"C:\github\Fx.Core\ComplexTerminalMultipleTypesTemplate.txt");
            var escapedTemplate = template
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("{{0}}", "{0}")
                .Replace("{{1}}", "{1}")
                .Replace("{{2}}", "{2}")
                .Replace("{{3}}", "{3}")
                .Replace("{{4}}", "{4}")
                .Replace("{{5}}", "{5}")
                .Replace("{{6}}", "{6}")
                .Replace("{{7}}", "{7}")
                .Replace("{{8}}", "{8}")
                .Replace("{{9}}", "{9}")
                .Replace("{{10}}", "{10}")
                .Replace("{{11}}", "{11}")
                ;

            var generated = string.Format(
                escapedTemplate,
                operation,
                overload,
                overloadReturnType,
                overloadTypeParameters,
                overloadParameters,
                overloadGenericTypeConstraints,
                resultType,
                resultCast,
                arguments,
                defaultResult,
                customResult,
                operation.ToLower()
                );

            var unescapedGenerated = generated
                .Replace("{{", "{")
                .Replace("}}", "}");
            System.IO.File.WriteAllText($@"C:\github\Fx.Core\Fx.Core.Tests\System\Linq\V2\V2EnumerableUnitTests_{overload}.cs", unescapedGenerated);
        }

        private static void GenerateComplexMultipleTypesTerminal()
        {
            GenerateComplexMultipleTypesTerminal(
                operation: "ToDictionary",
                overload: "ToDictionaryWithKeySelector",
                overloadReturnType: "Dictionary<TKey, object>",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector",
                overloadGenericTypeConstraints: "where TKey : notnull",
                resultType: "Dictionary<object, object>",
                resultCast: "IDictionary<TKey, object>",
                arguments: "_ => _",
                defaultResult: "new Dictionary<object, object>()",
                customResult: "new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) })"
                );
            GenerateComplexMultipleTypesTerminal(
                operation: "ToDictionary",
                overload: "ToDictionaryWithKeySelectorElementSelectorAndComparer",
                overloadReturnType: "Dictionary<TKey, TElement>",
                overloadTypeParameters: "<TKey, TElement>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer",
                overloadGenericTypeConstraints: "where TKey : notnull",
                resultType: "Dictionary<object, object>",
                resultCast: "IDictionary<TKey, TElement>",
                arguments: "_ => _, _ => _, EqualityComparer<object>.Default",
                defaultResult: "new Dictionary<object, object>()",
                customResult: "new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) })"
                );
            GenerateComplexMultipleTypesTerminal(
                operation: "ToDictionary",
                overload: "ToDictionaryWithKeySelectorAndElementSelector",
                overloadReturnType: "Dictionary<TKey, TElement>",
                overloadTypeParameters: "<TKey, TElement>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector",
                overloadGenericTypeConstraints: "where TKey : notnull",
                resultType: "Dictionary<object, object>",
                resultCast: "IDictionary<TKey, TElement>",
                arguments: "_ => _, _ => _",
                defaultResult: "new Dictionary<object, object>()",
                customResult: "new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) })"
                );
            GenerateComplexMultipleTypesTerminal(
                operation: "ToDictionary",
                overload: "ToDictionaryWithKeySelectorAndComparer",
                overloadReturnType: "Dictionary<TKey, object>",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                overloadGenericTypeConstraints: "where TKey : notnull",
                resultType: "Dictionary<object, object>",
                resultCast: "IDictionary<TKey, object>",
                arguments: "_ => _, EqualityComparer<object>.Default",
                defaultResult: "new Dictionary<object, object>()",
                customResult: "new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) })"
                );
        }

        [TestMethod]
        public void Generate()
        {
            GenerateFluent();
            GenerateTerminal();
            GenerateTyped();
            GenerateComplexTerminal();
            GenerateComplexMultipleTypesTerminal();

            //// TODO icastablemixin; you never really figured out the design for this
            //// TODO ioftypeable; this is suposed to be non-generic...
            //// TODO either implement monad checks or remove entirely orderby and orderbydescending; then implement test cases for them //// TODO here's a through for this category of mixins: you could have the v2 variants be 1:1 with the v1 variants, but underlying, we still use the enumerable monad, but we don't create a new e.g. orderbymonad; it's also possible that you do this at a later time (create a work item and move on)
            //// TODO either implement monad checks or remove entirely tolookup; then implement test cases for it (//// TODO revmoe groupby if you do remove this)

            //// TODO move mixin tests to their own folder or give them a name prefix
            //// TODO change the existing test class to be v1 tests
            //// TODO use the base test class name for the things that are "infrastructure" (like booladapter)
            //// TODO cover the remaining branches non-v1 branches
            //// TODO cover the remaining v1 branches
            //// TODO figure out how you want to add this code generation to the repo for real (t4 or something?)
            //// TODO look at todos elsewhere
            //// TODO code quality?
        }

        private static void GenerateTerminal()
        {
            GenerateTerminal(
                operation: "Aggregate",
                overload: "Aggregate",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, object, object> func",
                resultType: "object",
                arguments: "(first, second) => singleton",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Aggregate",
                overload: "AggregateWithSeed",
                overloadReturnType: "TAccumulate",
                overloadTypeParameters: "<TAccumulate>",
                overloadParameters: "TAccumulate seed, Func<TAccumulate, object, TAccumulate> func",
                resultType: "object",
                arguments: "new object(), (first, second) => singleton",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Aggregate",
                overload: "AggregateWithSelector",
                overloadReturnType: "TResult",
                overloadTypeParameters: "<TAccumulate, TResult>",
                overloadParameters: "TAccumulate seed, Func<TAccumulate, object, TAccumulate> func, Func<TAccumulate, TResult> resultSelector",
                resultType: "object",
                arguments: "new object(), (first, second) => singleton, accumulate => singleton",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "All",
                overload: "All",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "BoolAdapter",
                arguments: "element => (BoolAdapter)(singleton.GetHashCode())",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Any",
                overload: "Any",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "BoolAdapter",
                arguments: "",
                sourceElementCount: "Element.GetHashCode() % 2"
                );
            GenerateTerminal(
                operation: "Any",
                overload: "AnyWithPredicate",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "BoolAdapter",
                arguments: "element => (BoolAdapter)(singleton.GetHashCode())",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithIntSelector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int> selector",
                resultType: "double",
                arguments: "element => singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithNullableIntSelector",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int?> selector",
                resultType: "double?",
                arguments: "element => (int?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithDecimalSelector",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal> selector",
                resultType: "decimal",
                arguments: "element => (decimal)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithDoubleSelector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double> selector",
                resultType: "double",
                arguments: "element => (double)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithNullableSingleSelector",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float?> selector",
                resultType: "float?",
                arguments: "element => (float?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithNullableInt64Selector",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long?> selector",
                resultType: "double?",
                arguments: "element => (long?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithSingleSelector",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float> selector",
                resultType: "float",
                arguments: "element => (float)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithNullableDoubleSelector",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double?> selector",
                resultType: "double?",
                arguments: "element => (double?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithInt64Selector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long> selector",
                resultType: "double",
                arguments: "element => (long)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Average",
                overload: "AverageWithNullableDecimalSelector",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal?> selector",
                resultType: "decimal?",
                arguments: "element => (decimal?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Contains",
                overload: "ContainsWithComparer",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "object value, IEqualityComparer<object>? comparer",
                resultType: "BoolAdapter",
                arguments: "BoolAdapter.True, BoolAdapter.Comparer.Instance",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Contains",
                overload: "Contains",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "object value",
                resultType: "BoolAdapter",
                arguments: "BoolAdapter.True",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Count",
                overload: "Count",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "int",
                arguments: "",
                sourceElementCount: "Element.GetHashCode()"
                );
            GenerateTerminal(
                operation: "Count",
                overload: "CountWithPredicate",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "int",
                arguments: "element => true",
                sourceElementCount: "Element.GetHashCode()"
                );

            GenerateTerminal(
                operation: "ElementAt",
                overload: "ElementAtIndex",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Index index",
                resultType: "object",
                arguments: "new Index(0)",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "ElementAt",
                overload: "ElementAt",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "int index",
                resultType: "object",
                arguments: "0",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "ElementAtOrDefault",
                overload: "ElementAtOrDefaultIndex",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "Index index",
                resultType: "object?",
                arguments: "new Index(0)",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "ElementAtOrDefault",
                overload: "ElementAtOrDefault",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "int index",
                resultType: "object?",
                arguments: "0",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "First",
                overload: "First",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "First",
                overload: "FirstWithPredicate",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object",
                arguments: "element => true",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "FirstOrDefault",
                overload: "FirstOrDefault",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object?",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "FirstOrDefault",
                overload: "FirstOrDefaultWithPredicate",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object?",
                arguments: "element => true",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "FirstOrDefault",
                overload: "FirstOrDefaultWithPredicateAndDefault",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate, object defaultValue",
                resultType: "object",
                arguments: "element => true, singleton",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "FirstOrDefault",
                overload: "FirstOrDefaultWithDefault",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "object defaultValue",
                resultType: "object",
                arguments: "singleton",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Last",
                overload: "Last",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Last",
                overload: "LastWithPredicate",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object",
                arguments: "element => true",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "LastOrDefault",
                overload: "LastOrDefault",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object?",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "LastOrDefault",
                overload: "LastOrDefaultWithPredicateAndDefault",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate, object defaultValue",
                resultType: "object",
                arguments: "element => true, singleton",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "LastOrDefault",
                overload: "LastOrDefaultWithPredicate",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object?",
                arguments: "element => true",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "LastOrDefault",
                overload: "LastOrDefaultWithDefault",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "object defaultValue",
                resultType: "object",
                arguments: "singleton",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "LongCount",
                overload: "LongCountWithPredicate",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "long",
                arguments: "element => true",
                sourceElementCount: "Element.GetHashCode()"
                );
            GenerateTerminal(
                operation: "LongCount",
                overload: "LongCount",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "long",
                arguments: "",
                sourceElementCount: "Element.GetHashCode()"
                );

            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithInt64Selector",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long> selector",
                resultType: "long",
                arguments: "element => (long)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithNullableInt64Selector",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long?> selector",
                resultType: "long?",
                arguments: "element => (long?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithNullableSingleSelector",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float?> selector",
                resultType: "float?",
                arguments: "element => (float?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithNullableInt32Selector",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int?> selector",
                resultType: "int?",
                arguments: "element => (int?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithComparer",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "IComparer<object>? comparer",
                resultType: "object?",
                arguments: "Comparer<object>.Default",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithNullableDecimalSelector",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal?> selector",
                resultType: "decimal?",
                arguments: "element => (decimal?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithInt32Selector",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int> selector",
                resultType: "int",
                arguments: "element => (int)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithDoubleSelector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double> selector",
                resultType: "double",
                arguments: "element => (double)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithDecimalSelector",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal> selector",
                resultType: "decimal",
                arguments: "element => (decimal)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithSelector",
                overloadReturnType: "TResult?",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, TResult> selector",
                resultType: "object?",
                arguments: "element => element",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithSingleSelector",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float> selector",
                resultType: "float",
                arguments: "element => (float)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "Max",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object?",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Max",
                overload: "MaxWithNullableDoubleSelector",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double?> selector",
                resultType: "double?",
                arguments: "element => (double?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "MaxBy",
                overload: "MaxBy",
                overloadReturnType: "object?",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector",
                resultType: "object?",
                arguments: "element => element",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "MaxBy",
                overload: "MaxByWithComparer",
                overloadReturnType: "object?",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector, IComparer<TKey>? comparer",
                resultType: "object?",
                arguments: "element => element, Comparer<object>.Default",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Min",
                overload: "MinWithInt32Selector",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int> selector",
                resultType: "int",
                arguments: "element => (int)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithInt64Selector",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long> selector",
                resultType: "long",
                arguments: "element => (long)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithNullableDecimalSelector",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal?> selector",
                resultType: "decimal?",
                arguments: "element => (decimal?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithNullableDoubleSelector",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double?> selector",
                resultType: "double?",
                arguments: "element => (double?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithNullableInt32Selector",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int?> selector",
                resultType: "int?",
                arguments: "element => (int?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithNullableSingleSelector",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float?> selector",
                resultType: "float?",
                arguments: "element => (float?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithSingleSelector",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float> selector",
                resultType: "float",
                arguments: "element => (float)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithSelector",
                overloadReturnType: "TResult?",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, TResult> selector",
                resultType: "object?",
                arguments: "element => element",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithDoubleSelector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double> selector",
                resultType: "double",
                arguments: "element => (double)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithNullableInt64Selector",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long?> selector",
                resultType: "long?",
                arguments: "element => (long?)singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "Min",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object?",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithComparer",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "IComparer<object>? comparer",
                resultType: "object?",
                arguments: "Comparer<object>.Default",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Min",
                overload: "MinWithDecimalSelector",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal> selector",
                resultType: "decimal",
                arguments: "element => (decimal)singleton.GetHashCode()",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "MinBy",
                overload: "MinByWithComparer",
                overloadReturnType: "object?",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector, IComparer<TKey>? comparer",
                resultType: "object?",
                arguments: "element => element, Comparer<object>.Default",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "MinBy",
                overload: "MinBy",
                overloadReturnType: "object?",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector",
                resultType: "object?",
                arguments: "element => element",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "SequenceEqual",
                overload: "SequenceEqual",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second",
                resultType: "BoolAdapter",
                arguments: "new[] { BoolAdapter.True }.ToV2Enumerable()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "SequenceEqual",
                overload: "SequenceEqualWithComparer",
                overloadReturnType: "bool",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second, IEqualityComparer<object>? comparer",
                resultType: "BoolAdapter",
                arguments: "new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Single",
                overload: "Single",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Single",
                overload: "SingleWithPredicate",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object",
                arguments: "element => true",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "SingleOrDefault",
                overload: "SingleOrDefaultWithPredicateAndDefaultValue",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate, object defaultValue",
                resultType: "object",
                arguments: "element => true, singleton.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "SingleOrDefault",
                overload: "SingleOrDefaultWithPredicate",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                resultType: "object?",
                arguments: "element => true",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "SingleOrDefault",
                overload: "SingleOrDefault",
                overloadReturnType: "object?",
                overloadTypeParameters: "",
                overloadParameters: "",
                resultType: "object?",
                arguments: "",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "SingleOrDefault",
                overload: "SingleOrDefaultWithDefaultValue",
                overloadReturnType: "object",
                overloadTypeParameters: "",
                overloadParameters: "object defaultValue",
                resultType: "object",
                arguments: "singleton.GetHashCode()",
                sourceElementCount: "1"
                );

            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsAsInt32s",
                overloadReturnType: "int",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int> selector",
                resultType: "int",
                arguments: "element => (int)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsAsNullableDoubles",
                overloadReturnType: "double?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double?> selector",
                resultType: "double?",
                arguments: "element => (double?)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithSingleSelector",
                overloadReturnType: "float",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float> selector",
                resultType: "float",
                arguments: "element => (float)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithNullableSingleSelector",
                overloadReturnType: "float?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, float?> selector",
                resultType: "float?",
                arguments: "element => (float?)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithNullableInt32Selector",
                overloadReturnType: "int?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int?> selector",
                resultType: "int?",
                arguments: "element => (int?)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithDoubleSelector",
                overloadReturnType: "double",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, double> selector",
                resultType: "double",
                arguments: "element => (double)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithNullableInt64Selector",
                overloadReturnType: "long?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long?> selector",
                resultType: "long?",
                arguments: "element => (long?)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsWithNullableDecimalSelector",
                overloadReturnType: "decimal?",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal?> selector",
                resultType: "decimal?",
                arguments: "element => (decimal?)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsAsInt64s",
                overloadReturnType: "long",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, long> selector",
                resultType: "long",
                arguments: "element => (long)element.GetHashCode()",
                sourceElementCount: "1"
                );
            GenerateTerminal(
                operation: "Sum",
                overload: "SumElementsAsDecimals",
                overloadReturnType: "decimal",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, decimal> selector",
                resultType: "decimal",
                arguments: "element => (decimal)element.GetHashCode()",
                sourceElementCount: "1"
                );
        }

        private static void GenerateTerminal(
            string operation,
            string overload,
            string overloadReturnType,
            string overloadTypeParameters,
            string overloadParameters,
            string resultType,
            string arguments,
            string sourceElementCount)
        {
            var template = System.IO.File.ReadAllText(@"C:\github\Fx.Core\TerminalTemplate.txt");
            var escapedTemplate = template
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("{{0}}", "{0}")
                .Replace("{{1}}", "{1}")
                .Replace("{{2}}", "{2}")
                .Replace("{{3}}", "{3}")
                .Replace("{{4}}", "{4}")
                .Replace("{{5}}", "{5}")
                .Replace("{{6}}", "{6}")
                .Replace("{{7}}", "{7}")
                .Replace("{{8}}", "{8}")
                ;

            var generated = string.Format(
                escapedTemplate,
                operation,
                overload,
                overloadReturnType,
                overloadTypeParameters,
                overloadParameters,
                resultType,
                arguments,
                sourceElementCount,
                operation.ToLower()
                );

            var unescapedGenerated = generated
                .Replace("{{", "{")
                .Replace("}}", "}");
            System.IO.File.WriteAllText($@"C:\github\Fx.Core\Fx.Core.Tests\System\Linq\V2\V2EnumerableUnitTests_{overload}.cs", unescapedGenerated);
        }

        private static void GenerateFluent()
        {
            GenerateFluent(
                operation: "Append",
                overload: "Append",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "object element",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "(object)this",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Chunk",
                overload: "Chunk",
                overloadReturnTypeParameters: "object[]",
                overloadTypeParameters: "",
                overloadParameters: "int size",
                monadType: "<object[]>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object[]",
                arguments: "17",
                collectionComparer: ", GroupingComparer.Instance"
                );

            GenerateFluent(
                operation: "Concat",
                overload: "Concat",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>()",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "DefaultIfEmpty",
                overload: "DefaultIfEmpty",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "DefaultIfEmpty",
                overload: "DefaultIfEmptyWithDefaultValue",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "object defaultValue",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "(object)this",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Distinct",
                overload: "Distinct",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Distinct",
                overload: "DistinctWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IEqualityComparer<object>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "DistinctBy",
                overload: "DistinctBy",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "element => element",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "DistinctBy",
                overload: "DistinctByWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "element => element, null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Except",
                overload: "Except",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Except",
                overload: "ExceptWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second, IEqualityComparer<object>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "ExceptBy",
                overload: "ExceptBy",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<TKey> second, Func<object, TKey> keySelector",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), element => element",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "ExceptBy",
                overload: "ExceptByWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), element => element, null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithElementSelectorAndResultSelectorAndComparer",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TKey, TElement, TResult>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => element, element => element, (key, elements) => (object)this, null",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithElementSelectorAndResultSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TKey, TElement, TResult>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => element, element => element, (key, elements) => (object)this",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithResultSelectorAndComparer",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TKey, TResult>",
                overloadParameters: "Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => element, (key, elements) => (object)this, null",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithResultSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TKey, TResult>",
                overloadParameters: "Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => element, (key, elements) => (object)this",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupBy",
                overloadReturnTypeParameters: "IV2Grouping<TKey, object>",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector",
                monadType: "<IV2Grouping<object, object>>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TKey>",
                resultReturnTypeParameters: "IV2Grouping<TKey, object>",
                arguments: "element => element",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithElementSelector",
                overloadReturnTypeParameters: "IV2Grouping<TKey, TElement>",
                overloadTypeParameters: "<TKey, TElement>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector",
                monadType: "<IV2Grouping<object, object>>",
                resultTypeArguments: "<object, object>",
                resultTypeParameters: "<TKey, TElement>",
                resultReturnTypeParameters: "IV2Grouping<TKey, TElement>",
                arguments: "element => element, element => element",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithComparer",
                overloadReturnTypeParameters: "IV2Grouping<TKey, object>",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                monadType: "<IV2Grouping<object, object>>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TKey>",
                resultReturnTypeParameters: "IV2Grouping<TKey, object>",
                arguments: "element => element, null",
                collectionComparer: ", GroupingComparer.Instance"
                );
            GenerateFluent(
                operation: "GroupBy",
                overload: "GroupByWithElementSelectorAndComparer",
                overloadReturnTypeParameters: "IV2Grouping<TKey, TElement>",
                overloadTypeParameters: "<TKey, TElement>",
                overloadParameters: "Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer",
                monadType: "<IV2Grouping<object, object>>",
                resultTypeArguments: "<object, object>",
                resultTypeParameters: "<TKey, TElement>",
                resultReturnTypeParameters: "IV2Grouping<TKey, TElement>",
                arguments: "element => element, element => element, null",
                collectionComparer: ", GroupingComparer.Instance"
                );

            GenerateFluent(
                operation: "GroupJoin",
                overload: "GroupJoinWithComparer",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TInner, TKey, TResult>",
                overloadParameters: "IV2Enumerable<TInner> inner, Func<object, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<object, IV2Enumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "V2Enumerable.Empty<object>(), outer => outer, inner => inner, (outer, inners) => (object)this, null",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "GroupJoin",
                overload: "GroupJoin",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TInner, TKey, TResult>",
                overloadParameters: "IV2Enumerable<TInner> inner, Func<object, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<object, IV2Enumerable<TInner>, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "V2Enumerable.Empty<object>(), outer => outer, inner => inner, (outer, inners) => (object)this",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Intersect",
                overload: "IntersectWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second, IEqualityComparer<object>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), null",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Intersect",
                overload: "Intersect",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>()",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "IntersectBy",
                overload: "IntersectBy",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<TKey> second, Func<object, TKey> keySelector",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), element => element",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "IntersectBy",
                overload: "IntersectByWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "V2Enumerable.Empty<object>(), element => element, null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Join",
                overload: "Join",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TInner, TKey, TResult>",
                overloadParameters: "IV2Enumerable<TInner> inner, Func<object, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<object, TInner, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "V2Enumerable.Empty<object>(), outer => outer, inner => inner, (outer, inner) => (object)this",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Join",
                overload: "JoinWithComparer",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TInner, TKey, TResult>",
                overloadParameters: "IV2Enumerable<TInner> inner, Func<object, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<object, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "V2Enumerable.Empty<object>(), outer => outer, inner => inner, (outer, inner) => (object)this, null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Prepend",
                overload: "Prepend",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "object element",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "this",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Reverse",
                overload: "Reverse",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Select",
                overload: "SelectWithIndexSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, int, TResult> selector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "(element, index) => element",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Select",
                overload: "Select",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, TResult> selector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => element",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "SelectMany",
                overload: "SelectManyWithIndexSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, int, IV2Enumerable<TResult>> selector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "(element, index) => V2Enumerable.Empty<object>()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "SelectMany",
                overload: "SelectManyWithCollectionAndResultSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TCollection, TResult>",
                overloadParameters: "Func<object, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => V2Enumerable.Empty<object>(), (element, collection) => collection",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "SelectMany",
                overload: "SelectManyWithIndexAndResultSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TCollection, TResult>",
                overloadParameters: "Func<object, int, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "(element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "SelectMany",
                overload: "SelectMany",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TResult>",
                overloadParameters: "Func<object, IV2Enumerable<TResult>> selector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "element => V2Enumerable.Empty<object>()",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Skip",
                overload: "Skip",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "int count",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "4",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "SkipLast",
                overload: "SkipLast",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "int count",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "4",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "SkipWhile",
                overload: "SkipWhile",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "element => true",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "SkipWhile",
                overload: "SkipWhileWithIndexPredicate",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "(element, index) => true",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Take",
                overload: "TakeWithRange",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Range range",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "new Range(new Index(2), new Index(5))",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Take",
                overload: "Take",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "int count",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "4",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "TakeLast",
                overload: "TakeLast",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "int count",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "4",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "TakeWhile",
                overload: "TakeWhile",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "element => true",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "TakeWhile",
                overload: "TakeWhileWithIndexPredicate",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "(element, index) => true",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Union",
                overload: "Union",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "new[] { string.Empty }.ToV2Enumerable()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Union",
                overload: "UnionWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "IV2Enumerable<object> second, IEqualityComparer<object>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "new[] { string.Empty }.ToV2Enumerable(), null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "UnionBy",
                overload: "UnionBy",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<object> second, Func<object, TKey> keySelector",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "new[] { string.Empty }.ToV2Enumerable(), (element) => new object()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "UnionBy",
                overload: "UnionByWithComparer",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "<TKey>",
                overloadParameters: "IV2Enumerable<object> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "new[] { string.Empty }.ToV2Enumerable(), (element) => new object(), null",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Where",
                overload: "Where",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "element => true",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Where",
                overload: "WhereWithIndexPredicate",
                overloadReturnTypeParameters: "object",
                overloadTypeParameters: "",
                overloadParameters: "Func<object, int, bool> predicate",
                monadType: "<object>",
                resultTypeArguments: "",
                resultTypeParameters: "",
                resultReturnTypeParameters: "object",
                arguments: "(element, index) => true",
                collectionComparer: string.Empty
                );

            GenerateFluent(
                operation: "Zip",
                overload: "ZipWithThird",
                overloadReturnTypeParameters: "(object First, TSecond Second, TThird Third)",
                overloadTypeParameters: "<TSecond, TThird>",
                overloadParameters: "IV2Enumerable<TSecond> second, IV2Enumerable<TThird> third",
                monadType: "<(object, string, string)>",
                resultTypeArguments: "<string, string>",
                resultTypeParameters: "<TSecond, TThird>",
                resultReturnTypeParameters: "(object, TSecond, TThird)",
                arguments: "new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Zip",
                overload: "Zip",
                overloadReturnTypeParameters: "(object First, TSecond Second)",
                overloadTypeParameters: "<TSecond>",
                overloadParameters: "IV2Enumerable<TSecond> second",
                monadType: "<(object, string)>",
                resultTypeArguments: "<string>",
                resultTypeParameters: "<TSecond>",
                resultReturnTypeParameters: "(object, TSecond)",
                arguments: "new[] { string.Empty }.ToV2Enumerable()",
                collectionComparer: string.Empty
                );
            GenerateFluent(
                operation: "Zip",
                overload: "ZipWithResultSelector",
                overloadReturnTypeParameters: "TResult",
                overloadTypeParameters: "<TSecond, TResult>",
                overloadParameters: "IV2Enumerable<TSecond> second, Func<object, TSecond, TResult> resultSelector",
                monadType: "<object>",
                resultTypeArguments: "<object>",
                resultTypeParameters: "<TResult>",
                resultReturnTypeParameters: "TResult",
                arguments: "new[] { string.Empty }.ToV2Enumerable(), (first, second) => (object)this",
                collectionComparer: string.Empty
                );
        }

        private static void GenerateFluent(
            string operation,
            string overload,
            string overloadReturnTypeParameters,
            string overloadTypeParameters,
            string overloadParameters,
            string monadType,
            string resultTypeArguments,
            string resultTypeParameters,
            string resultReturnTypeParameters,
            string arguments,
            string collectionComparer)
        {
            var template = System.IO.File.ReadAllText(@"C:\github\Fx.Core\FluentTemplate.txt");
            var escapedTemplate = template
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("{{0}}", "{0}")
                .Replace("{{1}}", "{1}")
                .Replace("{{2}}", "{2}")
                .Replace("{{3}}", "{3}")
                .Replace("{{4}}", "{4}")
                .Replace("{{5}}", "{5}")
                .Replace("{{6}}", "{6}")
                .Replace("{{7}}", "{7}")
                .Replace("{{8}}", "{8}")
                .Replace("{{9}}", "{9}")
                .Replace("{{10}}", "{10}")
                .Replace("{{11}}", "{11}")
                ;

            var generated = string.Format(
                escapedTemplate,
                operation,
                overload,
                operation.ToLower(),
                overloadReturnTypeParameters,
                overloadTypeParameters,
                overloadParameters,
                monadType,
                resultTypeArguments,
                resultTypeParameters,
                resultReturnTypeParameters,
                arguments,
                collectionComparer
                );

            var unescapedGenerated = generated
                .Replace("{{", "{")
                .Replace("}}", "}");
            System.IO.File.WriteAllText($@"C:\github\Fx.Core\Fx.Core.Tests\System\Linq\V2\V2EnumerableUnitTests_{overload}.cs", unescapedGenerated);
        }

        [TestMethod]
        public void Spike()
        {
            var current = Enumerable.Empty<object>().GetEnumerator().Current;
            var @string = Enumerable.Empty<string>().GetEnumerator().Current;
            var @int = Enumerable.Empty<int>().GetEnumerator().Current;

            var moved = Enumerable.Empty<object>().GetEnumerator().MoveNext();

            var data = new RavudetExtension<string>(new[] { "asdf" }.ToV2Enumerable());
            SpikeExtensions.Driver<RavudetExtension<string>, string>(data);
            var driven = data.Driver<RavudetExtension<string>, string>();
        }

        /// <summary>
        /// Confirm that the monad gets passed through after operations
        /// </summary>
        [TestMethod]
        public void Test0()
        {
            var original = new[] { 10 }.ToV2Enumerable();
            var foo = new FooExtension<int>(original).AsV2Enumerable();

            var appended = foo.Append(5);
            CollectionAssert.AreEqual(new[] { 5, 10 }, appended.ToArray());

            var prependedAppended = appended.Prepend(15);
            CollectionAssert.AreEqual(new[] { 15, 5, 10 }, prependedAppended.ToArray());

            var appendedPrependedAppended = prependedAppended.Append(20);
            CollectionAssert.AreEqual(new[] { 20, 15, 5, 10 }, appendedPrependedAppended.ToArray());

            var appendedAppendedPrependedAppended = appendedPrependedAppended.Append(25);
            CollectionAssert.AreEqual(new[] { 25, 20, 15, 5, 10 }, appendedAppendedPrependedAppended.ToArray());
        }

        /// <summary>
        /// Confirm that two monads can work together and both get passed through after operations on each
        /// </summary>
        [TestMethod]
        public void Test()
        {
            var original = new[] { 10 }.ToV2Enumerable();
            var foo = new FooExtension<int>(original).AsV2Enumerable();
            var bar = new BarExtension<int>(foo).AsV2Enumerable();

            // append should still delegate to "FooExtension"
            var appended = bar.Append(5);
            CollectionAssert.AreEqual(new[] { 5, 10 }, appended.ToArray());

            // prepend should still delegate to "BarExtension" even though "FooExtension" is what returned to us from the last call
            var prependedAppended = appended.Prepend(15);
            CollectionAssert.AreEqual(new[] { 5, 10, 15 }, prependedAppended.ToArray());
            
            // and now even though "BarExtension" is what just returned to us, we should still be calling "FooExtension"
            var appendedPrependedAppended = prependedAppended.Append(20);
            CollectionAssert.AreEqual(new[] { 20, 5, 10, 15 }, appendedPrependedAppended.ToArray());

            // and to close the loop, even though we have called through each extension once, we should still be able to get to the correct extension
            var prependedAppendedPrependedAppended = appendedPrependedAppended.Prepend(25);
            CollectionAssert.AreEqual(new[] { 20, 5, 10, 15, 25 }, prependedAppendedPrependedAppended.ToArray());
        }

        /// <summary>
        /// Confirm that the monad gets passed through after operations outside of the monad
        /// </summary>
        [TestMethod]
        public void Test2()
        {
            var original = new[] { 5, 10 }.ToV2Enumerable();
            var foo = new FooExtension<int>(original).AsV2Enumerable();

            var whered = foo.Where(_ => _ % 2 == 0);
            CollectionAssert.AreEqual(new[] { 10 }, whered.ToArray());

            var appendedWhered = whered.Append(15);
            CollectionAssert.AreEqual(new[] { 15, 10 }, appendedWhered.ToArray());
        }

        /// <summary>
        /// Confirm that a mixin gets passed through with a monad and a monad gets passed through with a mixin
        /// </summary>
        [TestMethod]
        public void Test3()
        {
            var getContainingTypes = () => new[]
            {
                new ContainingType(1, new ContainedType("asdf")),
                new ContainingType(3, new ContainedType("qwerzvx")),
                new ContainingType(2, new ContainedType("1234")),
            };
            var containedTypeCallCount = 0;
            var getContainedTypes = (ContainingType containingType) =>
            {
                ++containedTypeCallCount;
                return Enumerable.Repeat(containingType.ContainedType, containingType.Id);
            };
            var whereable = new WhereableRetrieval(getContainingTypes, getContainedTypes).AsV2Enumerable();

            // check the correct behavior with no where applied
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("asdf"),
                    new ContainedType("qwerzvx"),
                    new ContainedType("qwerzvx"),
                    new ContainedType("qwerzvx"),
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                },
                whereable.ToArray(),
                ContainedTypeComparer.Instance);
            Assert.AreEqual(3, containedTypeCallCount);

            // check the correct behavior with a where applied
            containedTypeCallCount = 0;
            var whered = whereable.Where(containedType => containedType.SomeProperty.Length % 2 == 0);
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("asdf"),
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                },
                whered.ToArray(),
                ContainedTypeComparer.Instance);
            Assert.AreEqual(2, containedTypeCallCount);

            // now observe that we lose the call count optimization when a concact is applied
            containedTypeCallCount = 0;
            var concat1 = new[] { new ContainedType("hjkl") }.ToV2Enumerable();
            var wheredConcated = whereable.Concat(concat1).Where(containedType => containedType.SomeProperty.Length % 2 == 0);
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("asdf"),
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                    new ContainedType("hjkl"),
                },
                wheredConcated.ToArray(),
                ContainedTypeComparer.Instance);
            Assert.AreEqual(3, containedTypeCallCount);

            // confirm that concatextension preserves the call count optimization
            var extended = new ConcatExtension<ContainedType>(whereable).AsV2Enumerable();
            containedTypeCallCount = 0;
            var wheredConcatedExtended = extended.Concat(concat1).Where(containedType => containedType.SomeProperty.Length % 2 == 0);
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("asdf"),
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                    new ContainedType("hjkl"),
                },
                wheredConcatedExtended.ToArray(),
                ContainedTypeComparer.Instance);
            Assert.AreEqual(2, containedTypeCallCount);

            // confirm that *subsequent* concats still preserve the call count optimization
            containedTypeCallCount = 0;
            var concat2 = new[] { new ContainedType("yuio") }.ToV2Enumerable();
            var wheredConcatedWheredConcatedExtended = extended
                .Concat(concat1)
                .Where(containedType => containedType.SomeProperty.Length % 2 == 0)
                .Concat(concat2)
                .Where(containedType => containedType.SomeProperty.Contains("1"));
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                },
                wheredConcatedWheredConcatedExtended.ToArray(),
                ContainedTypeComparer.Instance);
            Assert.AreEqual(1, containedTypeCallCount);
        }

        private sealed class ConcatExtension<T> : IEnumerableMonad<T>, IConcatableMixin<T>, IWhereableMixin<T>
        {
            public ConcatExtension(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return source => new ConcatExtension<TSource>(source);
            }

            IV2Enumerable<T> IConcatableMixin<T>.Concat(IV2Enumerable<T> second)
            {
                return new Concated(this.Source, second);
            }

            private sealed class Concated : IWhereableMixin<T>
            {
                private readonly IV2Enumerable<T> source;
                private readonly IV2Enumerable<T> second;

                public Concated(IV2Enumerable<T> source, IV2Enumerable<T> second)
                {
                    this.source = source;
                    this.second = second;
                }

                public IV2Enumerable<T> Where(Func<T, bool> predicate)
                {
                    return new Whered(this.source, this.second, predicate);
                }

                private sealed class Whered : IV2Enumerable<T>
                {
                    private readonly IV2Enumerable<T> source;
                    private readonly IV2Enumerable<T> second;
                    private readonly Func<T, bool> predicate;

                    public Whered(IV2Enumerable<T> source, IV2Enumerable<T> second, Func<T, bool> predicate)
                    {
                        this.source = source;
                        this.second = second;
                        this.predicate = predicate;
                    }

                    public IEnumerator<T> GetEnumerator()
                    {
                        return this.source.Where(this.predicate).Concat(this.second.Where(this.predicate)).GetEnumerator();
                    }

                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        return this.GetEnumerator();
                    }
                }

                public IEnumerator<T> GetEnumerator()
                {
                    return this.source.Concat(this.second).GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }
            }

            public IV2Enumerable<T> Where(Func<T, bool> predicate)
            {
                return new Whered(this.Source, predicate);
            }

            private sealed class Whered : IWhereableMixin<T>
            {
                private readonly IV2Enumerable<T> source;
                private readonly Func<T, bool> predicate;

                public Whered(IV2Enumerable<T> source, Func<T, bool> predicate)
                {
                    this.source = source;
                    this.predicate = predicate;
                }

                public IV2Enumerable<T> Where(Func<T, bool> predicate)
                {
                    return new Whered(this.source, (element) => this.predicate(element) && predicate(element));
                }

                public IEnumerator<T> GetEnumerator()
                {
                    return this.source.Where(this.predicate).GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }

        private sealed class WhereableRetrieval : IWhereableMixin<ContainedType>
        {
            private readonly Func<IEnumerable<ContainingType>> getContainingTypes;
            private readonly Func<ContainingType, IEnumerable<ContainedType>> getContainedTypes;

            public WhereableRetrieval(Func<IEnumerable<ContainingType>> getContainingTypes, Func<ContainingType, IEnumerable<ContainedType>> getContainedTypes)
            {
                this.getContainingTypes = getContainingTypes;
                this.getContainedTypes = getContainedTypes;
            }

            public IV2Enumerable<ContainedType> Where(Func<ContainedType, bool> predicate)
            {
                return new Whered(this.getContainingTypes, this.getContainedTypes, predicate);
            }

            private sealed class Whered : IV2Enumerable<ContainedType>
            {
                private readonly Func<IEnumerable<ContainingType>> getContainingTypes;
                private readonly Func<ContainingType, IEnumerable<ContainedType>> getContainedTypes;
                private readonly Func<ContainedType, bool> predicate;

                public Whered(
                    Func<IEnumerable<ContainingType>> getContainingTypes, 
                    Func<ContainingType, IEnumerable<ContainedType>> getContainedTypes, 
                    Func<ContainedType, bool> predicate)
                {
                    this.getContainingTypes = getContainingTypes;
                    this.getContainedTypes = getContainedTypes;
                    this.predicate = predicate;
                }

                public IEnumerator<ContainedType> GetEnumerator()
                {
                    var containingTypes = this.getContainingTypes();
                    var whered = containingTypes.Where(containingType => this.predicate(containingType.ContainedType));
                    return whered.SelectMany(getContainedTypes).GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }
            }

            public IEnumerator<ContainedType> GetEnumerator()
            {
                return this.getContainingTypes().SelectMany(this.getContainedTypes).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private sealed class ContainingType
        {
            public ContainingType(int id, ContainedType containedType)
            {
                this.Id = id;
                this.ContainedType = containedType;
            }

            public int Id { get; }

            public ContainedType ContainedType { get; }
        }

        private sealed class ContainedTypeComparer : IComparer<ContainedType>, IComparer
        {
            private ContainedTypeComparer()
            {
            }

            public static ContainedTypeComparer Instance { get; } = new ContainedTypeComparer();

            public int Compare(ContainedType? x, ContainedType? y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                return string.Compare(x.SomeProperty, y.SomeProperty);
            }

            public int Compare(object? x, object? y)
            {
                if (x is ContainedType containedTypeX && y is ContainedType containedTypeY)
                {
                    return this.Compare(containedTypeX, containedTypeY);
                }

                return Comparer.Default.Compare(x, y);
            }
        }

        private sealed class ContainedType
        {
            public ContainedType(string someProperty)
            {
                this.SomeProperty = someProperty;
            }

            public string SomeProperty { get; }
        }

        private sealed class FooExtension<T> : IEnumerableMonad<T>, IAppendableMixin<T>
        {
            public FooExtension(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return source => new FooExtension<TSource>(source);
            }

            public IV2Enumerable<T> Append(T element)
            {
                var appended = AppendIterator(element).ToV2Enumerable();
                return appended;
            }

            private IEnumerable<T> AppendIterator(T element)
            {
                yield return element;
                foreach (var sourceElement in this.Source)
                {
                    yield return sourceElement;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }

        private sealed class BarExtension<T> : IEnumerableMonad<T>, IPrependableMixin<T>
        {
            public BarExtension(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return source => new BarExtension<TSource>(source);
            }

            public IV2Enumerable<T> Prepend(T element)
            {
                var prepended = this.PrependIterator(element).ToV2Enumerable();
                return prepended;
            }

            private IEnumerable<T> PrependIterator(T element)
            {
                foreach (var sourceElement in this.Source)
                {
                    yield return sourceElement;
                }

                yield return element;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }
    }
}
