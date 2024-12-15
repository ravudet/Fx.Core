namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void LongCountMixinWithOverload()
        {
            var enumerable = new MockLongCountMixinWithOverload().AsV2Enumerable();
            var singleton = MockLongCountMixinWithOverload.Result;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountMixinWithOverload : ILongCountableMixin<object>
        {
            public static long Result { get; } = new object().GetHashCode();

            public long LongCount()
            {
                return (long)Result;
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
        public void LongCountMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsMixin : ILongCountableMixin<object>, IEnumerableMonad<object>
        {
            public static long Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ILongCountableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public long LongCount()
                {
                    return (long)MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void LongCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ILongCountableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (long)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockLongCountMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, Element.GetHashCode()).ToV2Enumerable();

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
        public void LongCountMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockLongCountMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockLongCountMixinWithoutOverloadAndNoMonad.Element;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountMixinWithoutOverloadAndNoMonad : ILongCountableMixin<object>
        {
            public static object Element { get; } = (long)new object().GetHashCode();

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
        public void LongCountNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLongCountNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLongCountNoMixinAndMonadWhereSourceIsMixin.Result;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static long Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ILongCountableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public long LongCount()
                {
                    return (long)MockLongCountNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void LongCountNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLongCountNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLongCountNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (long)new object().GetHashCode();

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
        public void LongCountNoMixinAndNoMonad()
        {
            var enumerable = new MockLongCountNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockLongCountNoMixinAndNoMonad.Element;
            var longcounted = enumerable.LongCount();
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (long)new object().GetHashCode();

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
    }
}