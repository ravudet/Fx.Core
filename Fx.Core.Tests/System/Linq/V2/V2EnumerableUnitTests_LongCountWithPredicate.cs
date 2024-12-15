namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void LongCountWithPredicateMixinWithOverload()
        {
            var enumerable = new MockLongCountWithPredicateMixinWithOverload().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateMixinWithOverload.Result;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountWithPredicateMixinWithOverload : ILongCountableMixin<object>
        {
            public static long Result { get; } = new object().GetHashCode();

            public long LongCount(Func<object, bool> predicate)
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
        public void LongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin : ILongCountableMixin<object>, IEnumerableMonad<object>
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

                public long LongCount(Func<object, bool> predicate)
                {
                    return (long)MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void LongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ILongCountableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockLongCountWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, Element.GetHashCode()).ToV2Enumerable();

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
        public void LongCountWithPredicateMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockLongCountWithPredicateMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateMixinWithoutOverloadAndNoMonad.Element;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountWithPredicateMixinWithoutOverloadAndNoMonad : ILongCountableMixin<object>
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
        public void LongCountWithPredicateNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsMixin.Result;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton, longcounted);
        }

        private sealed class MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public long LongCount(Func<object, bool> predicate)
                {
                    return (long)MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void LongCountWithPredicateNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountWithPredicateNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void LongCountWithPredicateNoMixinAndNoMonad()
        {
            var enumerable = new MockLongCountWithPredicateNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockLongCountWithPredicateNoMixinAndNoMonad.Element;
            var longcounted = enumerable.LongCount(element => true);
            Assert.AreEqual<long>(singleton.GetHashCode(), longcounted);
        }

        private sealed class MockLongCountWithPredicateNoMixinAndNoMonad : IV2Enumerable<object>
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