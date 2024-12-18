namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        // mixin vs no mixin
        // overload vs no overload
        // monad vs no monad
        // source is mixin vs source is not mixin

/*
mixin   overload    monad   sourcemixin     nonsense                                                    terminal
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






mixin   overload    monad   sourcemixin     nonsense                                                    fluent
T       T           T       T               if there's an overload, the monad won't get called
T       T           T       F                                                                           MixinWithOverloadAndMonadWhereSourceIsNotMixin
T       T           F       T               if there's no monad, there's won't be a sourcemixin
T       T           F       F                                                                           MixinWithOverloadAndNoMonad
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






mixin   overload    monad   sourcemixin     nonsense                                                    typed
T       T           T       T               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           T       F               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           F       T               if there's no monad, there's won't be a sourcemixin
T       T           F       F
T       F           T       T
T       F           T       F
T       F           F       T               if there's no monad, there's won't be a sourcemixin
T       F           F       F
F       T           T       T               if there's no mixin, there can't be an overload
F       T           T       F               if there's no mixin, there can't be an overload
F       T           F       T               if there's no mixin, there can't be an overload
F       T           F       F               if there's no mixin, there can't be an overload
F       F           T       T
F       F           T       F
F       F           F       T               if there's no monad, there's won't be a sourcemixin
F       F           F       F

*/

        [TestMethod]
        public void AverageableDecimalMixinWithOverload()
        {
            var enumerable = new MockAverageableDecimalMixinWithOverload().AsV2Enumerable();
            var singleton = MockAverageableDecimalMixinWithOverload.Result;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton, ed);
        }

        private sealed class MockAverageableDecimalMixinWithOverload : IAverageableDecimalMixin
        {
            public static decimal Result { get; } = new object().GetHashCode();

            public decimal Average()
            {
                return (decimal)Result;
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

        /*[TestMethod]
        public void DecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton, ed);
        }

        private sealed class MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin : IAverageableMixin<object>, IEnumerableMonad<object>
        {
            public static decimal Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : IAverageableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal Average()
                {
                    return (decimal)MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void DecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton.GetHashCode(), ed);
        }

        private sealed class MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IAverageableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (decimal)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockDecimalMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, ).ToV2Enumerable();

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
        public void DecimalMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockDecimalMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockDecimalMixinWithoutOverloadAndNoMonad.Element;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton.GetHashCode(), ed);
        }

        private sealed class MockDecimalMixinWithoutOverloadAndNoMonad : IAverageableMixin<object>
        {
            public static object Element { get; } = (decimal)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < ; ++i)
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
        public void DecimalNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockDecimalNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockDecimalNoMixinAndMonadWhereSourceIsMixin.Result;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton, ed);
        }

        private sealed class MockDecimalNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static decimal Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : IAverageableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal Average()
                {
                    return (decimal)MockDecimalNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void DecimalNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockDecimalNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockDecimalNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton.GetHashCode(), ed);
        }

        private sealed class MockDecimalNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (decimal)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    for (int i = 0; i < ; ++i)
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
        public void DecimalNoMixinAndNoMonad()
        {
            var enumerable = new MockDecimalNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockDecimalNoMixinAndNoMonad.Element;
            var ed = enumerable.Average();
            Assert.AreEqual<decimal>(singleton.GetHashCode(), ed);
        }

        private sealed class MockDecimalNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (decimal)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < ; ++i)
                {
                    yield return Element;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }*/
    }
}