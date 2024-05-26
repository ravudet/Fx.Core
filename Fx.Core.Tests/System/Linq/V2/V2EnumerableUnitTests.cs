namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [TestClass]
    public class V2EnumerableUnitTests
    {
        //// TODO should you remove iv2 : iv1? that way no one accidentally escapes back to v1?
        //// TODO you skpped the adapter methods (tov2enumerable, tov2lookup, etc.)

        /// <summary>
        /// Aggregates a sequence
        /// </summary>
        [TestMethod]
        public void Aggregate()
        {
            Func<string, string, string> func = (_, _) => "asdf";
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.AsV2Enumerable().Aggregate(func);
            Assert.AreEqual("asdf", aggregated);

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Aggregate(func));
        }

        /// <summary>
        /// Aggregates a sequence using a seed
        /// </summary>
        [TestMethod]
        public void AggregateWithSeed()
        {
            var seed = "asdf";
            Func<string, string, string> func = (_, _) => "qwer";
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.AsV2Enumerable().Aggregate(seed, func);
            Assert.AreEqual("qwer", aggregated);

            // make sure v1 has different behavior
            Assert.AreEqual("asdf", enumerable.AsEnumerable().Aggregate(seed, func));
        }

        /// <summary>
        /// Aggregates a sequence using a seed and a result selector
        /// </summary>
        [TestMethod]
        public void AggregateWithSeedAndResultSelector()
        {
            var seed = "asdf";
            Func<string, string, string> func = (_, _) => "qwer";
            Func<string, string> resultSelector = _ => _;
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.AsV2Enumerable().Aggregate(seed, func, resultSelector);
            Assert.AreEqual("qwer", aggregated);

            // make sure v1 has different behavior
            Assert.AreEqual("asdf", enumerable.AsEnumerable().Aggregate(seed, func, resultSelector));
        }

        private sealed class AggregatableMock : IAggregatableMixin<string>
        {
            public AggregatableMock()
            {
            }
            public string Aggregate(Func<string, string, string> func)
            {
                return func(string.Empty, string.Empty);
            }

            public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Func<TAccumulate, string, TAccumulate> func)
            {
                return func(seed, string.Empty);
            }

            public TResult Aggregate<TAccumulate, TResult>(
                TAccumulate seed,
                Func<TAccumulate, string, TAccumulate> func,
                Func<TAccumulate, TResult> resultSelector)
            {
                return resultSelector(func(seed, string.Empty));
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Alls a sequence
        /// </summary>
        [TestMethod]
        public void All()
        {
            var called = false;
            Func<string, bool> predicate = _ => called = !called;
            var enumerable = new AllableMock();

            Assert.AreEqual(false, enumerable.AsV2Enumerable().All(predicate));
            Assert.AreEqual(true, called);

            // make sure v1 has different behavior
            Assert.AreEqual(true, enumerable.AsEnumerable().All(predicate));
            Assert.AreEqual(true, called); // v1 won't actually call the predicate, so 'called' remains unchanged
        }

        private sealed class AllableMock : IAllableMixin<string>
        {
            public AllableMock()
            {
            }

            public bool All(Func<string, bool> predicate)
            {
                predicate(string.Empty);
                return false;
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Anys a sequence
        /// </summary>
        [TestMethod]
        public void Any()
        {
            var enumerable = new AnyableMock();

            Assert.AreEqual(true, enumerable.AsV2Enumerable().Any());

            // make sure v1 has different behavior
            Assert.AreEqual(false, enumerable.AsEnumerable().Any());
        }

        /// <summary>
        /// Anys a sequence using a predicate
        /// </summary>
        [TestMethod]
        public void AnyWithPredicate()
        {
            var called = false;
            Func<string, bool> predicate = _ => called = !called;
            var enumerable = new AnyableMock();

            Assert.AreEqual(true, enumerable.AsV2Enumerable().Any(predicate));
            Assert.AreEqual(true, called);

            // make sure v1 has different behavior
            Assert.AreEqual(false, enumerable.AsEnumerable().Any(predicate));
            Assert.AreEqual(true, called); // v1 won't actually call the predicate, so 'called' remains unchanged
        }

        private sealed class AnyableMock : IAnyableMixin<string>
        {
            public AnyableMock()
            {
            }

            public bool Any()
            {
                return true;
            }

            public bool Any(Func<string, bool> predicate)
            {
                predicate(string.Empty);
                return true;
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Appends an element to a sequence
        /// </summary>
        [TestMethod]
        public void Append()
        {
            var element = "asdf";
            var enumerable = new AppendableMock();

            CollectionAssert.AreEqual(new[] { "asdf", "asdf" }, enumerable.AsV2Enumerable().Append(element).ToList());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().AsEnumerable().Append(element).ToList());
        }

        private sealed class AppendableMock : IAppendableMixin<string>
        {
            public AppendableMock()
            {
            }

            public IV2Enumerable<string> Append(string element)
            {
                return new[] { element, element }.ToV2Enumerable();
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        //// TODO skipped asv2enumerable

        /// <summary>
        /// Averages a sequence by projecting the elements to int32
        /// </summary>
        [TestMethod]
        public void AverageUsingInt32()
        {
            Func<string, int> selector = _ => 4;
            var enumerable = new AverageableMock();

            Assert.AreEqual(4, enumerable.AsV2Enumerable().Average(selector));
        }

        private sealed class AverageableMock : IAverageableMixin<string>
        {
            public AverageableMock()
            {
            }

            public double Average(Func<string, int> selector)
            {
                return selector(string.Empty);
            }

            public double? Average(Func<string, int?> selector)
            {
                return selector(string.Empty);
            }

            public decimal Average(Func<string, decimal> selector)
            {
                return selector(string.Empty);
            }

            public double Average(Func<string, double> selector)
            {
                return selector(string.Empty);
            }

            public float? Average(Func<string, float?> selector)
            {
                return selector(string.Empty);
            }

            public double? Average(Func<string, long?> selector)
            {
                return selector(string.Empty);
            }

            public float Average(Func<string, float> selector)
            {
                return selector(string.Empty);
            }

            public double? Average(Func<string, double?> selector)
            {
                return selector(string.Empty);
            }

            public double Average(Func<string, long> selector)
            {
                return selector(string.Empty);
            }

            public decimal? Average(Func<string, decimal?> selector)
            {
                return selector(string.Empty);
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}