namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void SumInt64sMixinWithOverload()
        {
            var enumerable = new MockSumInt64sMixinWithOverload().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sMixinWithOverload.Result, sumed);
        }

        private sealed class MockSumInt64sMixinWithOverload : ISumableMixin<object>
        {
            public static int Result { get; } = new object().GetHashCode();

            public long Sum(Func<object, long> selector)
            {
                return Result;
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
        public void SumInt64sMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result, sumed);
        }

        private sealed class MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISumableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISumableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public long Sum(Func<object, long> selector)
                {
                    return MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void SumInt64sMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element.GetHashCode(), sumed);
        }

        private sealed class MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISumableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = new[] { MockSumInt64sMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element }.ToV2Enumerable();

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
        public void SumInt64sMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSumInt64sMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sMixinWithoutOverloadAndNoMonad.Element.GetHashCode(), sumed);
        }

        private sealed class MockSumInt64sMixinWithoutOverloadAndNoMonad : ISumableMixin<object>
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
        public void SumInt64sNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSumInt64sNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sNoMixinAndMonadWhereSourceIsMixin.Result, sumed);
        }

        private sealed class MockSumInt64sNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISumableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public long Sum(Func<object, long> selector)
                {
                    return MockSumInt64sNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void SumInt64sNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSumInt64sNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sNoMixinAndMonadWhereSourceIsNotMixin.Element.GetHashCode(), sumed);
        }

        private sealed class MockSumInt64sNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void SumInt64sNoMixinAndNoMonad()
        {
            var enumerable = new MockSumInt64sNoMixinAndNoMonad().AsV2Enumerable();
            var sumed = enumerable.Sum(element => element.GetHashCode());
            Assert.AreEqual(MockSumInt64sNoMixinAndNoMonad.Element.GetHashCode(), sumed);
        }

        private sealed class MockSumInt64sNoMixinAndNoMonad : IV2Enumerable<object>
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