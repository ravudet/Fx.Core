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
        public void MaxableNullableDoubleMixinWithOverload()
        {
            var enumerable = new MockMaxableNullableDoubleMixinWithOverload().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleMixinWithOverload.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleMixinWithOverload : IMaxableNullableDoubleMixin
        {
            public static double? Result { get; } = new object().GetHashCode();

            public double? Max()
            {
                return (double?)Result;
            }

            public IEnumerator<double?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsMixin : IMaxableNullableDoubleMixin, IEnumerableMonad<double?>
        {
            public static double? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<double?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableDoubleMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public double? Max()
                {
                    return (double?)MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<double?> GetEnumerator()
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

            public IEnumerator<double?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMaxableNullableDoubleMixin, IEnumerableMonad<double?>
        {
            public static double? Element { get; } = (double?)new object().GetHashCode();

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

            public IV2Enumerable<double?> Source { get; } = Enumerable.Repeat(MockMaxableNullableDoubleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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

            public IEnumerator<double?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDoubleMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMaxableNullableDoubleMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleMixinWithoutOverloadAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleMixinWithoutOverloadAndNoMonad : IMaxableNullableDoubleMixin
        {
            public static double? Element { get; } = (double?)new object().GetHashCode();

            public IEnumerator<double?> GetEnumerator()
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
        public void MaxableNullableDoubleNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<double?>
        {
            public static double? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<double?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableDoubleMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public double? Max()
                {
                    return (double?)MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<double?> GetEnumerator()
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

            public IEnumerator<double?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDoubleNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<double?>
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

            public IV2Enumerable<double?> Source { get; } = SourceEnumerable.Instance;

            public static double? Element { get; } = (double?)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<double?>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<double?> GetEnumerator()
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

            public IEnumerator<double?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableDoubleNoMixinAndNoMonad()
        {
            var enumerable = new MockMaxableNullableDoubleNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableDoubleNoMixinAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<double?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableDoubleNoMixinAndNoMonad : IV2Enumerable<double?>
        {
            public static double? Element { get; } = (double?)new object().GetHashCode();

            public IEnumerator<double?> GetEnumerator()
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