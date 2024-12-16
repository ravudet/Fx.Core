namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void MinWithSelectorMixinWithOverload()
        {
            var enumerable = new MockMinWithSelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockMinWithSelectorMixinWithOverload.Result;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton, mined);
        }

        private sealed class MockMinWithSelectorMixinWithOverload : IMinableMixin<object>
        {
            public static object? Result { get; } = new object().GetHashCode();

            public TResult? Min<TResult>(Func<object, TResult> selector)
            {
                return (TResult?)Result;
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
        public void MinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton, mined);
        }

        private sealed class MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : IMinableMixin<object>, IEnumerableMonad<object>
        {
            public static object? Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : IMinableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public TResult? Min<TResult>(Func<object, TResult> selector)
                {
                    return (TResult?)MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void MinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMinableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (object?)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockMinWithSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void MinWithSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMinWithSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMinWithSelectorMixinWithoutOverloadAndNoMonad.Element;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithSelectorMixinWithoutOverloadAndNoMonad : IMinableMixin<object>
        {
            public static object Element { get; } = (object?)new object().GetHashCode();

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
        public void MinWithSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMinWithSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMinWithSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton, mined);
        }

        private sealed class MockMinWithSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static object? Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : IMinableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public TResult? Min<TResult>(Func<object, TResult> selector)
                {
                    return (TResult?)MockMinWithSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void MinWithSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMinWithSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMinWithSelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (object?)new object().GetHashCode();

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
        public void MinWithSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockMinWithSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMinWithSelectorNoMixinAndNoMonad.Element;
            var mined = enumerable.Min(element => element);
            Assert.AreEqual<object?>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithSelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (object?)new object().GetHashCode();

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