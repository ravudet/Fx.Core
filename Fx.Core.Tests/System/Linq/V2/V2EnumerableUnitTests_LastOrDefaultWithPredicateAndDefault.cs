namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void LastOrDefaultWithPredicateAndDefaultMixinWithOverload()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultMixinWithOverload().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultMixinWithOverload.Result;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultMixinWithOverload : ILastOrDefaultableMixin<object>
        {
            public static object Result { get; } = new object().GetHashCode();

            public object LastOrDefault(Func<object, bool> predicate, object defaultValue)
            {
                return (object)Result;
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
        public void LastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin : ILastOrDefaultableMixin<object>, IEnumerableMonad<object>
        {
            public static object Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ILastOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object LastOrDefault(Func<object, bool> predicate, object defaultValue)
                {
                    return (object)MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void LastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ILastOrDefaultableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (object)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void LastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndNoMonad.Element;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultMixinWithoutOverloadAndNoMonad : ILastOrDefaultableMixin<object>
        {
            public static object Element { get; } = (object)new object().GetHashCode();

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
        public void LastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static object Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ILastOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object LastOrDefault(Func<object, bool> predicate, object defaultValue)
                {
                    return (object)MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void LastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (object)new object().GetHashCode();

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
        public void LastOrDefaultWithPredicateAndDefaultNoMixinAndNoMonad()
        {
            var enumerable = new MockLastOrDefaultWithPredicateAndDefaultNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithPredicateAndDefaultNoMixinAndNoMonad.Element;
            var lastordefaulted = enumerable.LastOrDefault(element => true, singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithPredicateAndDefaultNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (object)new object().GetHashCode();

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