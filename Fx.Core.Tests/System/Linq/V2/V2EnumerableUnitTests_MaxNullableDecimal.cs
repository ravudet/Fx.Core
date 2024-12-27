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
        public void MaxableNullableDecimalMixinWithOverload()
        {
            var enumerable = new MockMaxableNullableDecimalMixinWithOverload().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalMixinWithOverload.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalMixinWithOverload : IMaxableNullableDecimalMixin
        {
            public static decimal? Result { get; } = new object().GetHashCode();

            public decimal? Max()
            {
                return (decimal?)Result;
            }

            public IEnumerator<decimal?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin : IMaxableNullableDecimalMixin, IEnumerableMonad<decimal?>
        {
            public static decimal? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<decimal?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableDecimalMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal? Max()
                {
                    return (decimal?)MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<decimal?> GetEnumerator()
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

            public IEnumerator<decimal?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMaxableNullableDecimalMixin, IEnumerableMonad<decimal?>
        {
            public static decimal? Element { get; } = (decimal?)new object().GetHashCode();

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

            public IV2Enumerable<decimal?> Source { get; } = Enumerable.Repeat(MockMaxableNullableDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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

            public IEnumerator<decimal?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDecimalMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMaxableNullableDecimalMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalMixinWithoutOverloadAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalMixinWithoutOverloadAndNoMonad : IMaxableNullableDecimalMixin
        {
            public static decimal? Element { get; } = (decimal?)new object().GetHashCode();

            public IEnumerator<decimal?> GetEnumerator()
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
        public void MaxableNullableDecimalNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<decimal?>
        {
            public static decimal? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<decimal?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableDecimalMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal? Max()
                {
                    return (decimal?)MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<decimal?> GetEnumerator()
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

            public IEnumerator<decimal?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDecimalNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<decimal?>
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

            public IV2Enumerable<decimal?> Source { get; } = SourceEnumerable.Instance;

            public static decimal? Element { get; } = (decimal?)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<decimal?>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<decimal?> GetEnumerator()
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

            public IEnumerator<decimal?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDecimalNoMixinAndNoMonad()
        {
            var enumerable = new MockMaxableNullableDecimalNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableDecimalNoMixinAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<decimal?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDecimalNoMixinAndNoMonad : IV2Enumerable<decimal?>
        {
            public static decimal? Element { get; } = (decimal?)new object().GetHashCode();

            public IEnumerator<decimal?> GetEnumerator()
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