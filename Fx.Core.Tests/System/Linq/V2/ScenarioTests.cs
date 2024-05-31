namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Reflection.Metadata.Ecma335;

    [TestClass]
    public sealed class ScenarioTests
    {
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
            var wheredConcated = whereable.Concat(new[] { new ContainedType("hjkl") }.ToV2Enumerable()).Where(containedType => containedType.SomeProperty.Length % 2 == 0);
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
            var wheredConcatedExtended = extended.Concat(new[] { new ContainedType("hjkl") }.ToV2Enumerable()).Where(containedType => containedType.SomeProperty.Length % 2 == 0);
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
            var concat1 = new ConcatExtension<ContainedType>(new[] { new ContainedType("hjkl") }.ToV2Enumerable()).AsV2Enumerable();
            var concat2 = new ConcatExtension<ContainedType>(new[] { new ContainedType("yuio") }.ToV2Enumerable()).AsV2Enumerable();

            var first = extended.Concat(concat1);
            var second = first.Where(containedType => containedType.SomeProperty.Length % 2 == 0);
            var third = second.Concat(concat2);
            var fourth = third.Where(containedType => containedType.SomeProperty.Contains("1"));

            /*var wheredConcatedWheredConcatedExtended = extended
                .Concat(concat1)
                .Where(containedType => containedType.SomeProperty.Length % 2 == 0)
                .Concat(concat2)
                .Where(containedType => containedType.SomeProperty.Contains("1"));*/
            CollectionAssert.AreEqual(
                new[]
                {
                    new ContainedType("1234"),
                    new ContainedType("1234"),
                },
                fourth.ToArray(),
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

            public IV2Enumerable<T> Concat(IV2Enumerable<T> second)
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
