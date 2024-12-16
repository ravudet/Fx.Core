namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void SumElementsWithDoubleSelectorMixinWithOverload()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorMixinWithOverload().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorMixinWithOverload.Result;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton, sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorMixinWithOverload : ISumableMixin<object>
        {
            public static double Result { get; } = new object().GetHashCode();

            public double Sum(Func<object, double> selector)
            {
                return (double)Result;
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
        public void SumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton, sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static double Result { get; } = new object().GetHashCode();

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

                public double Sum(Func<object, double> selector)
                {
                    return (double)MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISumableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (double)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void SumElementsWithDoubleSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorMixinWithoutOverloadAndNoMonad : ISumableMixin<object>
        {
            public static object Element { get; } = (double)new object().GetHashCode();

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
        public void SumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton, sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static double Result { get; } = new object().GetHashCode();

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

                public double Sum(Func<object, double> selector)
                {
                    return (double)MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void SumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (double)new object().GetHashCode();

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
        public void SumElementsWithDoubleSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockSumElementsWithDoubleSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockSumElementsWithDoubleSelectorNoMixinAndNoMonad.Element;
            var sumed = enumerable.Sum(element => (double)element.GetHashCode());
            Assert.AreEqual<double>(singleton.GetHashCode(), sumed);
        }

        private sealed class MockSumElementsWithDoubleSelectorNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (double)new object().GetHashCode();

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