namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void SumElementsWithNullableInt32SelectorMixinWithOverload()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorMixinWithOverload.Result;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorMixinWithOverload : ISumableMixin<object>
        {
            public static int? Result { get; } = new object().GetHashCode();

            public int? Sum(Func<object, int?> selector)
            {
                return (int?)Result;
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
        public void SumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static int? Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ISumableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public int? Sum(Func<object, int?> selector)
                {
                    return (int?)MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (int?)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void SumElementsWithNullableInt32SelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorMixinWithoutOverloadAndNoMonad : ISumableMixin<object>
        {
            public static object Element { get; } = (int?)new object().GetHashCode();

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
        public void SumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static int? Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ISumableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public int? Sum(Func<object, int?> selector)
                {
                    return (int?)MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (int?)new object().GetHashCode();

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
        public void SumElementsWithNullableInt32SelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockSumElementsWithNullableInt32SelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableInt32SelectorNoMixinAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (int?)element.GetHashCode());
            Assert.AreEqual<int?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableInt32SelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (int?)new object().GetHashCode();

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