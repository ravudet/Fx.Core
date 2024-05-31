namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [TestClass]
    public sealed class V2EnumerableUnitTests
    {
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

        /// <summary>
        /// Calls the same method on the same v2enumerable using different "views"
        /// </summary>
        [TestMethod]
        public void AsV2Enumerable()
        {
            var enumerable = new MockV2Enumerable<string>();

            Assert.AreEqual(1, MockView(enumerable));
            Assert.AreEqual(2, MockView(enumerable.AsV2Enumerable()));
        }

        private sealed class MockV2Enumerable<T> : IAnyableMixin<T>
        {
            public MockV2Enumerable()
            {
            }

            public IEnumerator<T> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private static int MockView<T>(MockV2Enumerable<T> mock)
        {
            return 1;
        }

        private static int MockView<T>(IV2Enumerable<T> v2)
        {
            return 2;
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Int32"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingInt32()
        {
            Func<string, int> selector = _ => 4;
            var enumerable = new AverageableMock();

            Assert.AreEqual(4, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Int64"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingInt64()
        {
            Func<string, long> selector = _ => 5;
            var enumerable = new AverageableMock();

            Assert.AreEqual(5, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Nullable{Double}"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingNullableDouble()
        {
            Func<string, double?> selector = _ => 6;
            var enumerable = new AverageableMock();

            Assert.AreEqual(6, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Single"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingSingle()
        {
            Func<string, float> selector = _ => 7;
            var enumerable = new AverageableMock();

            Assert.AreEqual(7, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Nullable{Int64}"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingNullableInt64()
        {
            Func<string, long?> selector = _ => 8;
            var enumerable = new AverageableMock();

            Assert.AreEqual(8, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Nullable{Single}"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingNullableSingle()
        {
            Func<string, float?> selector = _ => 9;
            var enumerable = new AverageableMock();

            Assert.AreEqual(9, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Double"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingDouble()
        {
            Func<string, double> selector = _ => 10;
            var enumerable = new AverageableMock();

            Assert.AreEqual(10, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Nullable{Int32}"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingNullableInt32()
        {
            Func<string, int?> selector = _ => 11;
            var enumerable = new AverageableMock();

            Assert.AreEqual(11, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Decimal"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingDecimal()
        {
            Func<string, decimal> selector = _ => 12;
            var enumerable = new AverageableMock();

            Assert.AreEqual(12, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
        }

        /// <summary>
        /// Averages a sequence by projecting the elements to <see cref="Nullable{Decimal}"/>
        /// </summary>
        [TestMethod]
        public void AverageUsingNullableDecimal()
        {
            Func<string, decimal> selector = _ => 13;
            var enumerable = new AverageableMock();

            Assert.AreEqual(13, enumerable.AsV2Enumerable().Average(selector));

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average(selector));
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

        /// <summary>
        /// Averages a sequence of <see cref="Nullable{Single}"/>
        /// </summary>
        [TestMethod]
        public void AverageNullableSingle()
        {
            var enumerable = new AverageableNullableSingleMock(14);

            Assert.AreEqual(14, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableNullableSingleMock : IAverageableNullableSingleMixin
        {
            private readonly float? average;

            public AverageableNullableSingleMock(float? average)
            {
                this.average = average;
            }

            public float? Average()
            {
                return this.average;
            }

            public IEnumerator<float?> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Nullable{Int64}"/>
        /// </summary>
        [TestMethod]
        public void AverageNullableInt64()
        {
            var enumerable = new AverageableNullableInt64Mock(15);

            Assert.AreEqual(15, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableNullableInt64Mock : IAverageableNullableInt64Mixin
        {
            private readonly double? average;

            public AverageableNullableInt64Mock(double? average)
            {
                this.average = average;
            }

            public double? Average()
            {
                return this.average;
            }

            public IEnumerator<long?> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Nullable{Int32}"/>
        /// </summary>
        [TestMethod]
        public void AverageNullableInt32()
        {
            var enumerable = new AverageableNullableInt32Mock(16);

            Assert.AreEqual(16, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableNullableInt32Mock : IAverageableNullableInt32Mixin
        {
            private readonly double? average;

            public AverageableNullableInt32Mock(double? average)
            {
                this.average = average;
            }

            public double? Average()
            {
                return this.average;
            }

            public IEnumerator<int?> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Nullable{Double}"/>
        /// </summary>
        [TestMethod]
        public void AverageNullableDouble()
        {
            var enumerable = new AverageableNullableDoubleMock(17);

            Assert.AreEqual(17, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableNullableDoubleMock : IAverageableNullableDoubleMixin
        {
            private readonly double? average;

            public AverageableNullableDoubleMock(double? average)
            {
                this.average = average;
            }

            public double? Average()
            {
                return this.average;
            }

            public IEnumerator<double?> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Nullable{Decimal}"/>
        /// </summary>
        [TestMethod]
        public void AverageNullableDecimal()
        {
            var enumerable = new AverageableNullableDecimalMock(18);

            Assert.AreEqual(18, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableNullableDecimalMock : IAverageableNullableDecimalMixin
        {
            private readonly decimal? average;

            public AverageableNullableDecimalMock(decimal? average)
            {
                this.average = average;
            }

            public decimal? Average()
            {
                return this.average;
            }

            public IEnumerator<decimal?> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Int64"/>
        /// </summary>
        [TestMethod]
        public void AverageInt64()
        {
            var enumerable = new AverageableInt64Mock(19);

            Assert.AreEqual(19, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableInt64Mock : IAverageableInt64Mixin
        {
            private readonly double average;

            public AverageableInt64Mock(double average)
            {
                this.average = average;
            }

            public double Average()
            {
                return this.average;
            }

            public IEnumerator<long> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Int32"/>
        /// </summary>
        [TestMethod]
        public void AverageInt32()
        {
            var enumerable = new AverageableInt32Mock(20);

            Assert.AreEqual(20, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableInt32Mock : IAverageableInt32Mixin
        {
            private readonly double average;

            public AverageableInt32Mock(double average)
            {
                this.average = average;
            }

            public double Average()
            {
                return this.average;
            }

            public IEnumerator<int> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Double"/>
        /// </summary>
        [TestMethod]
        public void AverageDouble()
        {
            var enumerable = new AverageableDoubleMock(21);

            Assert.AreEqual(21, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableDoubleMock : IAverageableDoubleMixin
        {
            private readonly double average;

            public AverageableDoubleMock(double average)
            {
                this.average = average;
            }

            public double Average()
            {
                return this.average;
            }

            public IEnumerator<double> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Decimal"/>
        /// </summary>
        [TestMethod]
        public void AverageDecimal()
        {
            var enumerable = new AverageableDecimalMock(22);

            Assert.AreEqual(22, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableDecimalMock : IAverageableDecimalMixin
        {
            private readonly decimal average;

            public AverageableDecimalMock(decimal average)
            {
                this.average = average;
            }

            public decimal Average()
            {
                return this.average;
            }

            public IEnumerator<decimal> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Averages a sequence of <see cref="Single"/>
        /// </summary>
        [TestMethod]
        public void AverageSingle()
        {
            var enumerable = new AverageableSingleMock(23);

            Assert.AreEqual(23, enumerable.AsV2Enumerable().Average());

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Average());
        }

        private sealed class AverageableSingleMock : IAverageableSingleMixin
        {
            private readonly float average;

            public AverageableSingleMock(float average)
            {
                this.average = average;
            }

            public float Average()
            {
                return this.average;
            }

            public IEnumerator<float> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Chunks a sequence
        /// </summary>
        [TestMethod]
        public void Chunk()
        {
            var size = 5;
            var enumerator = new ChunkableMock();

            var v2Chunked = enumerator.AsV2Enumerable().Chunk(size).ToArray();
            Assert.AreEqual(1, v2Chunked.Length);
            Assert.AreEqual(1, v2Chunked[0].Length);
            Assert.AreEqual("5", v2Chunked[0][0]);

            // make sure v1 has different behavior
            var v1Chunked = enumerator.AsEnumerable().Chunk(size).ToArray();
            Assert.AreEqual(0, v1Chunked.Length);
        }

        private sealed class ChunkableMock : IChunkableMixin<string>
        {
            public ChunkableMock()
            {
            }

            public IV2Enumerable<string[]> Chunk(int size)
            {
                return new[] { new[] { size.ToString() } }.ToV2Enumerable();
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

        //// TODO start a design document
        //// TODO document that you are not implementing thenby and thenbydescending yet because they are externally extensible and maybe benefit from a monad of their own
        //// TODO remind yourself that you could just use default interface implementations for all of linq; however, this doesn't establish the pattern for others to add their own extension methods and introduce their own interfaces; further, there can be conflicts between the interface and existing concrete collection implementations (or such things could be introduced in the future); those conflicts can cause compiler errors when upgrading linq versions in certain cases
        //// TODO implement empty; is emptydefault still relevant?
        //// TODO implement range; is rangedeafult still relevant?
        //// TODO implement repeat; is repeatdeafult still relevant?
        //// TODO address what you've written in chunkdefault
        //// TODO implement the monad check in the public extensions (where is an example)
        //// 
        //// TODO test that, for example, iaggregatablemixin does the right thing even if it only implements one of the overloads
        //// TODO you skpped tests for the the adapter methods (tov2enumerable, tov2lookup, etc.); you should have a separate implementation and test file for those
        ////
        //// TODO make sure the names of the variables make sense (like, you change from aggregatedoverload to monad, so the default extensions use the old name)
        //// TODO do you need the non-generic type? can you add it later?
        //// TODO add default implementation for icastablemixin if you keep the non-generic type
        //// TODO uncomment the cast extension method
        //// TODO add default implementation for ioftypeablemixin; you will want the mixin whether or not you keep the non-generic type, but it may look different if you keep the non-generic
        //// TODO uncomment the oftype extension method
        //// TODO should you remove iv2 : iv1? that way no one accidentally escapes back to v1?
        //// TODO if you remove v2: v1 then you should change asv2enumerable to asenumerable; otherwise, you should document in the design doc that having a different name is good to make it clear to callers which framework they are in
        //// TODO normalize on TElement everywhere
        //// TODO see where you can use covariance and contravariance; you maybe want to split up some mixin interfaces because doing so will give you granularity to define ins and outs that you otherwise couldn't
        //// TODO null checks
        //// TODO fix any spacing issues in the interface files in the overloads folder
        //// TODO check if you should make anything public that's internal or private

        //// TODO recording:
        //// open sound settings; make sure output and input are both the airpods hands-free
        //// https://app.clipchamp.com/
    }
}