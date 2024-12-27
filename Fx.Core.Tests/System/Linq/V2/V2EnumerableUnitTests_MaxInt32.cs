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
        public void MaxableInt32MixinWithOverload()
        {
            var enumerable = new MockMaxableInt32MixinWithOverload().AsV2Enumerable();
            var singleton = MockMaxableInt32MixinWithOverload.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32MixinWithOverload : IMaxableInt32Mixin
        {
            public static int Result { get; } = new object().GetHashCode();

            public int Max()
            {
                return (int)Result;
            }

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsMixin : IMaxableInt32Mixin, IEnumerableMonad<int>
        {
            public static int Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<int> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableInt32Mixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public int Max()
                {
                    return (int)MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<int> GetEnumerator()
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

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMaxableInt32Mixin, IEnumerableMonad<int>
        {
            public static int Element { get; } = (int)new object().GetHashCode();

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

            public IV2Enumerable<int> Source { get; } = Enumerable.Repeat(MockMaxableInt32MixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableInt32MixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMaxableInt32MixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableInt32MixinWithoutOverloadAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32MixinWithoutOverloadAndNoMonad : IMaxableInt32Mixin
        {
            public static int Element { get; } = (int)new object().GetHashCode();

            public IEnumerator<int> GetEnumerator()
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
        public void MaxableInt32NoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMaxableInt32NoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMaxableInt32NoMixinAndMonadWhereSourceIsMixin.Result;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32NoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<int>
        {
            public static int Result { get; } = new object().GetHashCode();

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

            public IV2Enumerable<int> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : IMaxableInt32Mixin
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public int Max()
                {
                    return (int)MockMaxableInt32NoMixinAndMonadWhereSourceIsMixin.Result;
                }

                public IEnumerator<int> GetEnumerator()
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

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableInt32NoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMaxableInt32NoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMaxableInt32NoMixinAndMonadWhereSourceIsNotMixin.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32NoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<int>
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

            public IV2Enumerable<int> Source { get; } = SourceEnumerable.Instance;

            public static int Element { get; } = (int)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<int>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<int> GetEnumerator()
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

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void MaxableInt32NoMixinAndNoMonad()
        {
            var enumerable = new MockMaxableInt32NoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMaxableInt32NoMixinAndNoMonad.Element;
            var maxed = enumerable.Max();
            Assert.AreEqual<int>(singleton, maxed);
        }

        private sealed class MockMaxableInt32NoMixinAndNoMonad : IV2Enumerable<int>
        {
            public static int Element { get; } = (int)new object().GetHashCode();

            public IEnumerator<int> GetEnumerator()
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