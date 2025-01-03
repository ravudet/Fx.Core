namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
/*
dimension under test:
1. mixin vs not a mixin
2. mixin overload is implemented vs mixin overload is not implement
3. monad is implemented vs monad is not implemented
4. monad source is mixin vs monad source is not mixin

mixin   overload    monad   sourcemixin     why the test sku doesn't make sense                         test name
T       T           T       T               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           T       F               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           F       T               if there's no monad, there's won't be a sourcemixin
T       T           F       F                                                                           MixinWithOverload
T       F           T       T                                                                           MixinWithoutOverloadAndMonadWhereSourceIsMixin
T       F           T       F                                                                           MixinWithoutOverloadAndMonadWhereSourceIsNotMixin
T       F           F       T               if there's no monad, there's won't be a sourcemixin
T       F           F       F                                                                           MixinWithoutOverloadAndNoMonad
F       T           T       T               if there's no mixin, there can't be an overload
F       T           T       F               if there's no mixin, there can't be an overload
F       T           F       T               if there's no mixin, there can't be an overload
F       T           F       F               if there's no mixin, there can't be an overload
F       F           T       T                                                                           NoMixinAndMonadWhereSourceIsMixin
F       F           T       F                                                                           NoMixinAndMonadWhereSourceIsNotMixin
F       F           F       T               if there's no monad, there's won't be a sourcemixin
F       F           F       F                                                                           NoMixinAndNoMonad
*/

        [TestMethod]
        public void SequenceEqualWithComparerMixinWithOverload()
        {
            var enumerable = new MockSequenceEqualWithComparerMixinWithOverload().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerMixinWithOverload.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerMixinWithOverload : ISequenceEqualableMixin<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

            public bool SequenceEqual(IV2Enumerable<object> second, IEqualityComparer<object>? comparer)
            {
                return (bool)Result;
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISequenceEqualableMixin<object>, IEnumerableMonad<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

            private static class ResultMonadFactory<T>
            {
                private static Unit<T> factory;

                static ResultMonadFactory()
                {
                    ResultMonad<T>.SetFactory();
                    if (factory == null)
                    {
                        throw new NotSupportedException("The SetFactory method did not set the factory");
                    }
                }

                public static Unit<T> Factory
                {
                    get
                    {
                        return factory;
                    }
                    set
                    {
                        if (factory != null)
                        {
                            throw new NotSupportedException("Cannot set the factory after initialization");
                        }

                        factory = value;
                    }
                }
            }

            public IV2Enumerable<object> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : ISequenceEqualableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool SequenceEqual(IV2Enumerable<object> second, IEqualityComparer<object>? comparer)
                {
                    return (bool)MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<object> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public Unit<TSource> Unit<TSource>()
            {
                return ResultMonadFactory<TSource>.Factory;
            }

            public sealed class ResultMonad<T> : IEnumerableMonad<T>
            {
                public static void SetFactory()
                {
                    ResultMonadFactory<T>.Factory = (IV2Enumerable<T> source) => new ResultMonad<T>(source);
                }

                private ResultMonad(IV2Enumerable<T> source)
                {
                    this.Source = source;
                }

                public IV2Enumerable<T> Source { get; }

                public Unit<TSource> Unit<TSource>()
                {
                    return ResultMonadFactory<TSource>.Factory;
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISequenceEqualableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode()!;

            private static class ResultMonadFactory<T>
            {
                private static Unit<T> factory;

                static ResultMonadFactory()
                {
                    ResultMonad<T>.SetFactory();
                    if (factory == null)
                    {
                        throw new NotSupportedException("The SetFactory method did not set the factory");
                    }
                }

                public static Unit<T> Factory
                {
                    get
                    {
                        return factory;
                    }
                    set
                    {
                        if (factory != null)
                        {
                            throw new NotSupportedException("Cannot set the factory after initialization");
                        }

                        factory = value;
                    }
                }
            }

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockSequenceEqualWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

            public Unit<TSource> Unit<TSource>()
            {
                return ResultMonadFactory<TSource>.Factory;
            }

            public sealed class ResultMonad<T> : IEnumerableMonad<T>
            {
                public static void SetFactory()
                {
                    ResultMonadFactory<T>.Factory = (IV2Enumerable<T> source) => new ResultMonad<T>(source);
                }

                private ResultMonad(IV2Enumerable<T> source)
                {
                    this.Source = source;
                }

                public IV2Enumerable<T> Source { get; }

                public Unit<TSource> Unit<TSource>()
                {
                    return ResultMonadFactory<TSource>.Factory;
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSequenceEqualWithComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerMixinWithoutOverloadAndNoMonad.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerMixinWithoutOverloadAndNoMonad : ISequenceEqualableMixin<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode()!;

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < 1; ++i)
                {
                    yield return Element;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsMixin.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

            private static class ResultMonadFactory<T>
            {
                private static Unit<T> factory;

                static ResultMonadFactory()
                {
                    ResultMonad<T>.SetFactory();
                    if (factory == null)
                    {
                        throw new NotSupportedException("The SetFactory method did not set the factory");
                    }
                }

                public static Unit<T> Factory
                {
                    get
                    {
                        return factory;
                    }
                    set
                    {
                        if (factory != null)
                        {
                            throw new NotSupportedException("Cannot set the factory after initialization");
                        }

                        factory = value;
                    }
                }
            }

            public IV2Enumerable<object> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : ISequenceEqualableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool SequenceEqual(IV2Enumerable<object> second, IEqualityComparer<object>? comparer)
                {
                    return (bool)MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<object> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public Unit<TSource> Unit<TSource>()
            {
                return ResultMonadFactory<TSource>.Factory;
            }

            public sealed class ResultMonad<T> : IEnumerableMonad<T>
            {
                public static void SetFactory()
                {
                    ResultMonadFactory<T>.Factory = (IV2Enumerable<T> source) => new ResultMonad<T>(source);
                }

                private ResultMonad(IV2Enumerable<T> source)
                {
                    this.Source = source;
                }

                public IV2Enumerable<T> Source { get; }

                public Unit<TSource> Unit<TSource>()
                {
                    return ResultMonadFactory<TSource>.Factory;
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
        {
            private static class ResultMonadFactory<T>
            {
                private static Unit<T> factory;

                static ResultMonadFactory()
                {
                    ResultMonad<T>.SetFactory();
                    if (factory == null)
                    {
                        throw new NotSupportedException("The SetFactory method did not set the factory");
                    }
                }

                public static Unit<T> Factory
                {
                    get
                    {
                        return factory;
                    }
                    set
                    {
                        if (factory != null)
                        {
                            throw new NotSupportedException("Cannot set the factory after initialization");
                        }

                        factory = value;
                    }
                }
            }

            public IV2Enumerable<object> Source { get; } = SourceEnumerable.Instance;

            public static object Element { get; } = (BoolAdapter)new object().GetHashCode()!;

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    for (int i = 0; i < 1; ++i)
                    {
                        yield return Element;
                    }
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }
            public Unit<TSource> Unit<TSource>()
            {
                return ResultMonadFactory<TSource>.Factory;
            }

            public sealed class ResultMonad<T> : IEnumerableMonad<T>
            {
                public static void SetFactory()
                {
                    ResultMonadFactory<T>.Factory = (IV2Enumerable<T> source) => new ResultMonad<T>(source);
                }

                private ResultMonad(IV2Enumerable<T> source)
                {
                    this.Source = source;
                }

                public IV2Enumerable<T> Source { get; }

                public Unit<TSource> Unit<TSource>()
                {
                    return ResultMonadFactory<TSource>.Factory;
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualWithComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockSequenceEqualWithComparerNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockSequenceEqualWithComparerNoMixinAndNoMonad.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable(), EqualityComparer<object>.Default);
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualWithComparerNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode()!;

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < 1; ++i)
                {
                    yield return Element;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}