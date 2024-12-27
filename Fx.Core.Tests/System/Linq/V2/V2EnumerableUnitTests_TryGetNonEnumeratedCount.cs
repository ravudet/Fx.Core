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
        public void TryGetNonEnumeratedCountMixinWithOverload()
        {
            var enumerable = new MockTryGetNonEnumeratedCountMixinWithOverload().AsV2Enumerable();
            var singleton = MockTryGetNonEnumeratedCountMixinWithOverload.Result;
            var counted = enumerable.TryGetNonEnumeratedCount(out var count);
            Assert.IsTrue(counted);
            Assert.AreEqual<int>(singleton, count);
        }

        private sealed class MockTryGetNonEnumeratedCountMixinWithOverload : ITryGetNonEnumeratedCountableMixin<object>
        {
            public static int Result { get; } = new object().GetHashCode();

            public bool TryGetNonEnumeratedCount(out int count)
            {
                count = (int)Result;
                return true;
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
        public void TryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var counted = enumerable.TryGetNonEnumeratedCount(out var count);
            Assert.IsTrue(counted);
            Assert.AreEqual<int>(singleton, count);
        }

        private sealed class MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsMixin : ITryGetNonEnumeratedCountableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : ITryGetNonEnumeratedCountableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool TryGetNonEnumeratedCount(out int count)
                {
                    count = (int)Result;
                    return true;
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
        public void TryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var counted = enumerable.TryGetNonEnumeratedCount(out var count);
            Assert.IsFalse(counted);
        }

        private sealed class MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ICountableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (int)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockTryGetNonEnumeratedCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, Element.GetHashCode()).ToV2Enumerable();

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
        public void TryGetNonEnumeratedCountMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockTryGetNonEnumeratedCountMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockTryGetNonEnumeratedCountMixinWithoutOverloadAndNoMonad.Element;
            var counted = enumerable.TryGetNonEnumeratedCount(out var count);
            Assert.IsFalse(counted);
        }

        private sealed class MockTryGetNonEnumeratedCountMixinWithoutOverloadAndNoMonad : ICountableMixin<object>
        {
            public static object Element { get; } = (int)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < Element.GetHashCode(); ++i)
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
        public void TryGetNonEnumeratedCountNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockTryGetNonEnumeratedCountNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockTryGetNonEnumeratedCountNoMixinAndMonadWhereSourceIsMixin.Result;
            var counted = enumerable.TryGetNonEnumeratedCount(out var count);
            Assert.IsTrue(counted);
            Assert.AreEqual<int>(singleton, count);
        }

        private sealed class MockTryGetNonEnumeratedCountNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = SourceEnumerable.Instance;

            private sealed class SourceEnumerable : ITryGetNonEnumeratedCountableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool TryGetNonEnumeratedCount(out int count)
                {
                    count = (int)MockTryGetNonEnumeratedCountNoMixinAndMonadWhereSourceIsMixin.Result;
                    return true;
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

        /*[TestMethod]
        public void CountNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockCountNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockCountNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var counted = enumerable.Count();
            Assert.AreEqual<int>(singleton.GetHashCode(), counted);
        }

        private sealed class MockCountNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (int)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    for (int i = 0; i < Element.GetHashCode(); ++i)
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
        public void CountNoMixinAndNoMonad()
        {
            var enumerable = new MockCountNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockCountNoMixinAndNoMonad.Element;
            var counted = enumerable.Count();
            Assert.AreEqual<int>(singleton.GetHashCode(), counted);
        }

        private sealed class MockCountNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (int)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < Element.GetHashCode(); ++i)
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