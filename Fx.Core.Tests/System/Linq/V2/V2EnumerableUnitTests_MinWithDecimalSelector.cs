namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void MinWithDecimalSelectorMixinWithOverload()
        {
            var enumerable = new MockMinWithDecimalSelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorMixinWithOverload.Result;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton, mined);
        }

        private sealed class MockMinWithDecimalSelectorMixinWithOverload : IMinableMixin<object>
        {
            public static decimal Result { get; } = new object().GetHashCode();

            public decimal Min(Func<object, decimal> selector)
            {
                return (decimal)Result;
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
        public void MinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton, mined);
        }

        private sealed class MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : IMinableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IMinableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal Min(Func<object, decimal> selector)
                {
                    return (decimal)MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void MinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IMinableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockMinWithDecimalSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void MinWithDecimalSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockMinWithDecimalSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorMixinWithoutOverloadAndNoMonad.Element;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithDecimalSelectorMixinWithoutOverloadAndNoMonad : IMinableMixin<object>
        {
            public static object Element { get; } = (decimal)new object().GetHashCode();

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
        public void MinWithDecimalSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton, mined);
        }

        private sealed class MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IMinableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public decimal Min(Func<object, decimal> selector)
                {
                    return (decimal)MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void MinWithDecimalSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithDecimalSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void MinWithDecimalSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockMinWithDecimalSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockMinWithDecimalSelectorNoMixinAndNoMonad.Element;
            var mined = enumerable.Min(element => (decimal)singleton.GetHashCode());
            Assert.AreEqual<decimal>(singleton.GetHashCode(), mined);
        }

        private sealed class MockMinWithDecimalSelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (decimal)new object().GetHashCode();

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