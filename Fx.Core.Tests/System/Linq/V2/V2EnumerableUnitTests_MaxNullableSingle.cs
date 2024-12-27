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
        public void MaxableNullableSingleMixinWithOverload()
        {
            var enumerable = new MockMaxableNullableSingleMixinWithOverload().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleMixinWithOverload.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleMixinWithOverload : IMaxableNullableSingleMixin
        {
            public static float? Result { get; } = new object().GetHashCode();

            public float? Max()
            {
                return (float?)Result;
            }

            public IEnumerator<float?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsMixin : IMaxableNullableSingleMixin, IEnumerableMonad<float?>
        {
            public static float? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<float?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableSingleMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public float? Max()
                {
                    return (float?)MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<float?> GetEnumerator()
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

            public IEnumerator<float?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMaxableNullableSingleMixin, IEnumerableMonad<float?>
        {
            public static float? Element { get; } = (float?)new object().GetHashCode();

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

            public IV2Enumerable<float?> Source { get; } = Enumerable.Repeat(MockMaxableNullableSingleMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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

            public IEnumerator<float?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableSingleMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMaxableNullableSingleMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleMixinWithoutOverloadAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleMixinWithoutOverloadAndNoMonad : IMaxableNullableSingleMixin
        {
            public static float? Element { get; } = (float?)new object().GetHashCode();

            public IEnumerator<float?> GetEnumerator()
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
        public void MaxableNullableSingleNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<float?>
        {
            public static float? Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<float?> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableNullableSingleMixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public float? Max()
                {
                    return (float?)MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<float?> GetEnumerator()
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

            public IEnumerator<float?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableSingleNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<float?>
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

            public IV2Enumerable<float?> Source { get; } = SourceEnumerable.Instance;

            public static float? Element { get; } = (float?)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<float?>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<float?> GetEnumerator()
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

            public IEnumerator<float?> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableNullableSingleNoMixinAndNoMonad()
        {
            var enumerable = new MockMaxableNullableSingleNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableNullableSingleNoMixinAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<float?>(singleton, maxed);
        }

        private sealed class MockMaxableNullableSingleNoMixinAndNoMonad : IV2Enumerable<float?>
        {
            public static float? Element { get; } = (float?)new object().GetHashCode();

            public IEnumerator<float?> GetEnumerator()
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