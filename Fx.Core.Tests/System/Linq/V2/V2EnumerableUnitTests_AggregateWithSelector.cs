namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void AggregateWithSelectorMixinWithOverload()
        {
            var enumerable = new MockAggregateWithSelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorMixinWithOverload.Result;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual((object)singleton, (object)aggregateed);
        }

        private sealed class MockAggregateWithSelectorMixinWithOverload : IAggregateableMixin<object>
        {
            public static object Result { get; } = new object().GetHashCode();

            public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Func<TAccumulate, object, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
            {
                return (TResult)Result;
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
        public void AggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual((object)singleton, (object)aggregateed);
        }

        private sealed class MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : IAggregateableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IAggregateableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Func<TAccumulate, object, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
                {
                    return (TResult)MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void AggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), aggregateed.GetHashCode());
        }

        private sealed class MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IAggregateableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = new object();

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

            public IV2Enumerable<object> Source { get; } = new[] { MockAggregateWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element }.ToV2Enumerable();

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
        public void AggregateWithSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockAggregateWithSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorMixinWithoutOverloadAndNoMonad.Element;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual(singleton.GetHashCode(), aggregateed.GetHashCode());
        }

        private sealed class MockAggregateWithSelectorMixinWithoutOverloadAndNoMonad : IAggregateableMixin<object>
        {
            public static object Element { get; } = new object();

            public IEnumerator<object> GetEnumerator()
            {
                yield return Element;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void AggregateWithSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual((object)singleton, (object)aggregateed);
        }

        private sealed class MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IAggregateableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Func<TAccumulate, object, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
                {
                    return (TResult)MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void AggregateWithSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual(singleton.GetHashCode(), aggregateed.GetHashCode());
        }

        private sealed class MockAggregateWithSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = new object();

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    yield return Element;
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
        public void AggregateWithSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockAggregateWithSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockAggregateWithSelectorNoMixinAndNoMonad.Element;
            var aggregateed = enumerable.Aggregate(new object(), (first, second) => singleton, accumulate => singleton);
            Assert.AreEqual(singleton.GetHashCode(), aggregateed.GetHashCode());
        }

        private sealed class MockAggregateWithSelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = new object();

            public IEnumerator<object> GetEnumerator()
            {
                yield return Element;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}