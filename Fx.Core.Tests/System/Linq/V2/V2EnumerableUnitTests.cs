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

        private sealed class MockV2Enumerable<T> : IV2Enumerable<T>
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
            var enumerable = new ChunkableMock();

            var v2Chunked = enumerable.AsV2Enumerable().Chunk(size).ToArray();
            Assert.AreEqual(1, v2Chunked.Length);
            Assert.AreEqual(1, v2Chunked[0].Length);
            Assert.AreEqual("5", v2Chunked[0][0]);

            // make sure v1 has different behavior
            var v1Chunked = enumerable.AsEnumerable().Chunk(size).ToArray();
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

        /// <summary>
        /// Concats two sequences
        /// </summary>
        [TestMethod]
        public void Concat()
        {
            var enumerable = new ConcatableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());
            var second = new[] { "zxcv", "1234" }.ToV2Enumerable();

            var v2Concated = enumerable.AsV2Enumerable().Concat(second);
            CollectionAssert.AreEqual(new[] { "zxcv", "1234", "asdf", "qwer" }, v2Concated.ToArray());

            // make sure v1 has different behavior
            var v1Concated = enumerable.AsEnumerable().Concat(second);
            CollectionAssert.AreEqual(new[] { "zxcv", "1234" }, v1Concated.ToArray());
        }

        private sealed class ConcatableMock : IConcatableMixin<string>
        {
            private readonly IV2Enumerable<string> first;

            public ConcatableMock(IV2Enumerable<string> first)
            {
                this.first = first;
            }

            public IV2Enumerable<string> Concat(IV2Enumerable<string> second)
            {
                return ConcatIterator(second).ToV2Enumerable();
            }
            
            private IEnumerable<string> ConcatIterator(IV2Enumerable<string> second)
            {
                foreach (var element in second)
                {
                    yield return element;
                }

                foreach (var element in this.first)
                {
                    yield return element;
                }
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
        /// Checks if a sequence contains an element
        /// </summary>
        [TestMethod]
        public void Contains()
        {
            var enumerable = new ContainsableMock();
            var value = "true";

            Assert.AreEqual(true, enumerable.AsV2Enumerable().Contains(value));

            // make sure v1 has different behavior
            Assert.AreEqual(false, enumerable.AsEnumerable().Contains(value));
        }

        /// <summary>
        /// Checks if a sequence contains an element using a comparer to determine equality of elements
        /// </summary>
        [TestMethod]
        public void ContainsWithComparer()
        {
            var enumerable = new ContainsableMock();
            var value = "false";
            var comparer = StringComparer.Ordinal;

            Assert.AreEqual(true, enumerable.AsV2Enumerable().Contains(value, comparer));

            // make sure v1 has different behavior
            Assert.AreEqual(false, enumerable.AsEnumerable().Contains(value, comparer));
        }

        /// <summary>
        /// Checks if a sequence contains an element using a <see langword="null"/> comparer to determine equality of elements
        /// </summary>
        [TestMethod]
        public void ContainsWithNullComparer()
        {
            var enumerable = new ContainsableMock();
            var value = "true";

            Assert.AreEqual(true, enumerable.AsV2Enumerable().Contains(value, null));

            // make sure v1 has different behavior
            Assert.AreEqual(false, enumerable.AsEnumerable().Contains(value, null));
        }

        private sealed class ContainsableMock : IContainsableMixin<string>
        {
            public bool Contains(string value, IEqualityComparer<string>? comparer)
            {
                return bool.Parse(value) == (comparer == null);
            }

            public bool Contains(string value)
            {
                return bool.Parse(value);
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
        /// Counts the element in a sequence
        /// </summary>
        [TestMethod]
        public void Count()
        {
            var enumerable = new CountableMock(4);

            Assert.AreEqual(4, enumerable.AsV2Enumerable().Count());

            // make sure v1 has different behavior
            Assert.AreEqual(0, enumerable.AsEnumerable().Count());
        }

        /// <summary>
        /// Counts the element in a sequence that match a predicate
        /// </summary>
        [TestMethod]
        public void CountWithPredicate()
        {
            var enumerable = new CountableMock(4);
            var predicate = (int element) => element % 2 == 0;

            Assert.AreEqual(1, enumerable.AsV2Enumerable().Count(predicate));

            // make sure v1 has different behavior
            Assert.AreEqual(0, enumerable.AsEnumerable().Count(predicate));
        }

        private sealed class CountableMock : ICountableMixin<int>
        {
            private readonly int count;

            public CountableMock(int count)
            {
                this.count = count;
            }

            public int Count()
            {
                return this.count;
            }

            public int Count(Func<int, bool> predicate)
            {
                return predicate(this.count) ? 1 : 0;
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
        /// Gets the sequence or its default if it's empty
        /// </summary>
        [TestMethod]
        public void DefaultIfEmpty()
        {
            var enumerable = new DefaultIfEmptyableMock("asdf");

            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().DefaultIfEmpty().ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(new string?[] { null }, enumerable.AsEnumerable().DefaultIfEmpty().ToArray());
        }

        /// <summary>
        /// Gets the sequence or its default if it's empty
        /// </summary>
        [TestMethod]
        public void DefaultIfEmptyWithDefault()
        {
            var enumerable = new DefaultIfEmptyableMock("asdf");
            var @default = "qwer";

            CollectionAssert.AreEqual(new[] { "asdf", "qwer" }, enumerable.AsV2Enumerable().DefaultIfEmpty(@default).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(new[] { "qwer" }, enumerable.AsEnumerable().DefaultIfEmpty(@default).ToArray());
        }

        private sealed class DefaultIfEmptyableMock : IDefaultIfEmptyableMixin<string>
        {
            private readonly string value;

            public DefaultIfEmptyableMock(string value)
            {
                this.value = value;
            }

            public IV2Enumerable<string?> DefaultIfEmpty()
            {
                return new[] { this.value }.ToV2Enumerable();
            }

            public IV2Enumerable<string> DefaultIfEmpty(string defaultValue)
            {
                return new[] { this.value, defaultValue }.ToV2Enumerable();
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
        /// Gets the distinct elements of a sequence
        /// </summary>
        [TestMethod]
        public void Distinct()
        {
            var enumerable = new DistinctableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());

            CollectionAssert.AreEqual(new[] { "asdf", "qwer" }, enumerable.AsV2Enumerable().Distinct().ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Distinct().ToArray());
        }

        /// <summary>
        /// Gets the distinct elements of a sequence using a comparer to determine element equality
        /// </summary>
        [TestMethod]
        public void DistinctWithComparer()
        {
            var enumerable = new DistinctableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());
            var comparer = StringComparer.OrdinalIgnoreCase;

            CollectionAssert.AreEqual(new[] { "qwer" }, enumerable.AsV2Enumerable().Distinct(comparer).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Distinct(comparer).ToArray());
        }

        /// <summary>
        /// Gets the distinct elements of a sequence using a <see langword="null"/> comparer to determine element equality
        /// </summary>
        [TestMethod]
        public void DistinctWithNullComparer()
        {
            var enumerable = new DistinctableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());

            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().Distinct(null).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Distinct(null).ToArray());
        }

        private sealed class DistinctableMock : IDistinctableMixin<string>
        {
            private readonly IV2Enumerable<string> values;

            public DistinctableMock(IV2Enumerable<string> values)
            {
                this.values = values;
            }

            public IV2Enumerable<string> Distinct()
            {
                return this.values;
            }

            public IV2Enumerable<string> Distinct(IEqualityComparer<string>? comparer)
            {
                return new[] { comparer == null ? this.values.First() : this.values.Last() }.ToV2Enumerable();
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
        /// Gets the distinct elements of a sequence using a selector to determine which portion of the elements should be used to determine equality
        /// </summary>
        [TestMethod]
        public void DistinctBy()
        {
            var enumerable = new DistinctByableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());
            var selector = (string element) => element;

            CollectionAssert.AreEqual(new[] { "aq" }, enumerable.AsV2Enumerable().DistinctBy(selector).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().DistinctBy(selector).ToArray());
        }

        /// <summary>
        /// Gets the distinct elements of a sequence using a selector to determine which portion of the elements should be used to determine equality and a comparer to determine the equality of those portions
        /// </summary>
        [TestMethod]
        public void DistinctByWithComparer()
        {
            var enumerable = new DistinctByableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());
            var selector = (string element) => element;
            var comparer = StringComparer.Ordinal;

            CollectionAssert.AreEqual(new[] { "qwer" }, enumerable.AsV2Enumerable().DistinctBy(selector, comparer).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().DistinctBy(selector, comparer).ToArray());
        }

        /// <summary>
        /// Gets the distinct elements of a sequence using a selector to determine which portion of the elements should be used to determine equality and a <see langword="null"/> comparer to determine the equality of those portions
        /// </summary>
        [TestMethod]
        public void DistinctByWithNullComparer()
        {
            var enumerable = new DistinctByableMock(new[] { "asdf", "qwer" }.ToV2Enumerable());
            var selector = (string element) => element;

            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().DistinctBy(selector, null).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().DistinctBy(selector, null).ToArray());
        }

        private sealed class DistinctByableMock : IDistinctByableMixin<string>
        {
            private readonly IV2Enumerable<string> values;

            public DistinctByableMock(IV2Enumerable<string> values)
            {
                this.values = values;
            }

            public IV2Enumerable<string> DistinctBy<TKey>(Func<string, TKey> keySelector)
            {
                var result = string.Empty;
                foreach (var element in this.values)
                {
                    var selected = keySelector(element);
                    if (selected as string == element)
                    {
                        result += element[0];
                    }
                }

                return new[] { result }.ToV2Enumerable();
            }

            public IV2Enumerable<string> DistinctBy<TKey>(Func<string, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            {
                var selected = keySelector(comparer == null ? this.values.First() : this.values.Last());
                return new[] { (selected as string) ?? string.Empty }.ToV2Enumerable();
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
        /// Gets the element at a certain index
        /// </summary>
        [TestMethod]
        public void ElementAt()
        {
            var enumerable = new ElementAtableMock();

            Assert.AreEqual("4", enumerable.AsV2Enumerable().ElementAt(4));

            // make sure v1 has different behavior
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.AsEnumerable().ElementAt(4));
        }

        /// <summary>
        /// Gets the element at a certain index
        /// </summary>
        [TestMethod]
        public void ElementAtIndex()
        {
            var enumerable = new ElementAtableMock();

            Assert.AreEqual("5", enumerable.AsV2Enumerable().ElementAt(new Index(5)));

            // make sure v1 has different behavior
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.AsEnumerable().ElementAt(new Index(5)));
        }

        private sealed class ElementAtableMock : IElementAtableMixin<string>
        {
            public string ElementAt(Index index)
            {
                return index.Value.ToString();
            }

            public string ElementAt(int index)
            {
                return index.ToString();
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
        /// Gets the element at a certain index
        /// </summary>
        [TestMethod]
        public void ElementAtOrDefault()
        {
            var enumerable = new ElementAtOrDefaultableMock();

            Assert.AreEqual("6", enumerable.AsV2Enumerable().ElementAtOrDefault(6));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().ElementAtOrDefault(6));
        }

        /// <summary>
        /// Gets the element at a certain index
        /// </summary>
        [TestMethod]
        public void ElementAtOrDefaultIndex()
        {
            var enumerable = new ElementAtOrDefaultableMock();

            Assert.AreEqual("7", enumerable.AsV2Enumerable().ElementAtOrDefault(new Index(7)));

            // make sure v1 has different behavior
            Assert.AreEqual(null, enumerable.AsEnumerable().ElementAtOrDefault(7));
        }

        private sealed class ElementAtOrDefaultableMock : IElementAtOrDefaultableMixin<string>
        {
            public string ElementAtOrDefault(Index index)
            {
                return index.Value.ToString();
            }

            public string ElementAtOrDefault(int index)
            {
                return index.ToString();
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
        /// Removes the elements of one sequence from another
        /// </summary>
        [TestMethod]
        public void Except()
        {
            var enumerable = new ExceptableMock();
            var second = new[] { "asdf", "qwer" }.ToV2Enumerable();

            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().Except(second).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Except(second).ToArray());
        }

        /// <summary>
        /// Removes the elements of one sequence from another using a comparer to determine equality of elements
        /// </summary>
        [TestMethod]
        public void ExceptWithComparer()
        {
            var enumerable = new ExceptableMock();
            var second = new[] { "asdf", "qwer" }.ToV2Enumerable();
            var comparer = StringComparer.Ordinal;

            CollectionAssert.AreEqual(new[] { "qwer" }, enumerable.AsV2Enumerable().Except(second, comparer).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Except(second, comparer).ToArray());
        }

        /// <summary>
        /// Removes the elements of one sequence from another using a <see langword="null"/> comparer to determine equality of elements
        /// </summary>
        [TestMethod]
        public void ExceptWithNullComparer()
        {
            var enumerable = new ExceptableMock();
            var second = new[] { "asdf", "qwer" }.ToV2Enumerable();

            CollectionAssert.AreEqual(new[] { "asdf" }, enumerable.AsV2Enumerable().Except(second, null).ToArray());

            // make sure v1 has different behavior
            CollectionAssert.AreEqual(Array.Empty<string>(), enumerable.AsEnumerable().Except(second, null).ToArray());
        }

        private sealed class ExceptableMock : IExceptableMixin<string>
        {
            public IV2Enumerable<string> Except(IV2Enumerable<string> second)
            {
                return new[] { second.First() }.ToV2Enumerable();
            }

            public IV2Enumerable<string> Except(IV2Enumerable<string> second, IEqualityComparer<string>? comparer)
            {
                return new[] { comparer == null ? second.First() : second.Last() }.ToV2Enumerable();
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
        /// Tests that the mixin implementation of ExceptBy is used
        /// </summary>
        [TestMethod]
        public void ExceptByableMixin()
        {
            var enumerable = new MockExceptByableMixin<string>().AsV2Enumerable();
            Assert.AreEqual(MockExceptByableMixin<string>.Result, enumerable.ExceptBy(V2Enumerable.Empty<string>(), _ => _));
        }

        /// <summary>
        /// Tests that the default implementation of ExceptBy is used when a mixin is not implemented
        /// </summary>
        [TestMethod]
        public void ExceptByableMixinDefaults()
        {
            var enumerable = new MockExceptByableMixin<string>().AsV2Enumerable();
            try
            {
                enumerable.ExceptBy(V2Enumerable.Empty<string>(), _ => _, null).Enumerate();
                Assert.Fail();
            }
            catch (Exception exception)
            {
                Assert.AreEqual(MockExceptByableMixin<string>.Exception, exception);
            }
        }

        private sealed class MockExceptByableMixin<TElement> : IExceptByableMixin<TElement>
        {
            public static IV2Enumerable<TElement> Result { get; } = ResultEnumerable<TElement>.Instance;

            private sealed class ResultEnumerable<TResult> : IV2Enumerable<TResult>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<TResult> Instance { get; } = new ResultEnumerable<TResult>();

                public IEnumerator<TResult> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public static Exception Exception { get; } = EnumerationException.Instance;

            private sealed class EnumerationException : Exception
            {
                private EnumerationException()
                    : base()
                {
                }

                public static EnumerationException Instance { get; } = new EnumerationException();
            }

            public IV2Enumerable<TElement> ExceptBy<TKey>(IV2Enumerable<TKey> second, Func<TElement, TKey> keySelector)
            {
                return Result;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                throw EnumerationException.Instance;
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Tests that the monad is preserved when a monad and a ExceptBy mixin are combined and the mixin implementation is called
        /// </summary>
        [TestMethod]
        public void ExceptByableMixinAndMonad()
        {
            var enumerable = new MockExceptByableMixinAndMonad<string>().AsV2Enumerable();
            var exceptByed = enumerable.ExceptBy(V2Enumerable.Empty<string>(), _ => _);
            var monad = exceptByed as MockExceptByableMixinAndMonad<string>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockExceptByableMixinAndMonad<string>.Result, monad.Source);
        }

        private sealed class MockExceptByableMixinAndMonad<TElement> : IExceptByableMixin<TElement>, IEnumerableMonad<TElement>
        {
            public MockExceptByableMixinAndMonad()
                : this(V2Enumerable.Empty<TElement>())
            {
            }

            private MockExceptByableMixinAndMonad(IV2Enumerable<TElement> source)
            {
                this.Source = source;
            }

            public static IV2Enumerable<TElement> Result { get; } = ResultEnumerable<TElement>.Instance;

            private sealed class ResultEnumerable<TResult> : IV2Enumerable<TResult>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<TResult> Instance { get; } = new ResultEnumerable<TResult>();

                public IEnumerator<TResult> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IV2Enumerable<TElement> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return (IV2Enumerable<TSource> source) => new MockExceptByableMixinAndMonad<TSource>(source);
            }

            public IV2Enumerable<TElement> ExceptBy<TKey>(IV2Enumerable<TKey> second, Func<TElement, TKey> keySelector)
            {
                return Result;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        [TestMethod]
        public void ExceptByDefault()
        {
            //// TODO
            V2Enumerable.Empty<string>().ExceptBy(V2Enumerable.Empty<string>(), _ => _).Enumerate();
        }

        /// <summary>
        /// Tests that the monad source is used when the source is the ExceptBy mixin
        /// </summary>
        [TestMethod]
        public void ExceptByMonad()
        {
            var enumerable = new MockExceptByMonad<string>(new MockExceptByMonadExceptByableMixin<string>().AsV2Enumerable()).AsV2Enumerable();
            var exceptByed = enumerable.ExceptBy(V2Enumerable.Empty<string>(), _ => _);
            var monad = exceptByed as MockExceptByMonad<string>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockExceptByMonadExceptByableMixin<string>.Result, monad.Source);
        }

        private sealed class MockExceptByMonadException<TElement> : Exception
        {
            public MockExceptByMonadException(IV2Enumerable<TElement> result)
                : base()
            {
                this.Result = result;
            }

            public IV2Enumerable<TElement> Result { get; }
        }

        private sealed class MockExceptByMonadExceptByableMixin<TElement> : IExceptByableMixin<TElement>
        {
            public static IV2Enumerable<TElement> Result { get; } = ResultEnumerable<TElement>.Instance;

            private sealed class ResultEnumerable<TResult> : IV2Enumerable<TResult>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<TResult> Instance { get; } = new ResultEnumerable<TResult>();

                public IEnumerator<TResult> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IV2Enumerable<TElement> ExceptBy<TKey>(IV2Enumerable<TKey> second, Func<TElement, TKey> keySelector)
            {
                return Result;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockExceptByMonad<TElement> : IEnumerableMonad<TElement>
        {
            public MockExceptByMonad(IV2Enumerable<TElement> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<TElement> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return (IV2Enumerable<TSource> source) => new MockExceptByMonad<TSource>(source);
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        //// TODO discuss design decision 3 with others; if you rename the interface methods, confusion can be avoided; also, having separate interfaces for every method avoids the need for the "default" behavior at all
        //// 
        //// TODO test that, for example, iaggregatablemixin does the right thing even if it only implements one of the overloads
        //// TODO you skpped tests for the the adapter methods (tov2enumerable, tov2lookup, etc.); you should have a separate implementation and test file for those
        ////
        //// TODO should the orderby variants use monad checks somehow? maybe not, maybe they should be treated list toarray and tolist?
        //// TODO should the tolookup variants use monad checks somehow? maybe not, maybe they should be treated list toarray and tolist?
        //// TODO should the factories actually be part of this release? you aren't allowing them to be extensible (for example, enumerable.repeat could be a countable mixin, but you're not doing that, and no one else can override that...); maybe the factories should be static interface methods? if they are static interface methods, then having "defaults" of them would make sense
        //// TODO should unit be a static method?
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
        //// TODO remove spike tests from ScenarioTests
        //// TODO productize scenario tests

        //// TODO recording:
        //// open sound settings; make sure output and input are both the airpods hands-free
        //// https://app.clipchamp.com/
    }

    internal static class TestTODO
    {
        public static IV2Enumerable<T> Enumerate<T>(this IV2Enumerable<T> source)
        {
            foreach (var element in source)
            {
            }

            return source;
        }
    }
}