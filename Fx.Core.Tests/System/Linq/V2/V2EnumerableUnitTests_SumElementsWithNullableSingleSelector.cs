namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void SumElementsWithNullableSingleSelectorMixinWithOverload()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorMixinWithOverload.Result;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorMixinWithOverload : ISumableMixin<object>
        {
            public static float? Result { get; } = new object().GetHashCode();

            public float? Sum(Func<object, float?> selector)
            {
                return (float?)Result;
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
        public void SumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static float? Result { get; } = new object().GetHashCode();

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

                public float? Sum(Func<object, float?> selector)
                {
                    return (float?)MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (float?)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void SumElementsWithNullableSingleSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorMixinWithoutOverloadAndNoMonad : ISumableMixin<object>
        {
            public static object Element { get; } = (float?)new object().GetHashCode();

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
        public void SumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton, sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static float? Result { get; } = new object().GetHashCode();

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

                public float? Sum(Func<object, float?> selector)
                {
                    return (float?)MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (float?)new object().GetHashCode();

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
        public void SumElementsWithNullableSingleSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockSumElementsWithNullableSingleSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithNullableSingleSelectorNoMixinAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (float?)element.GetHashCode());
            Assert.AreEqual<float?>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithNullableSingleSelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (float?)new object().GetHashCode();

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