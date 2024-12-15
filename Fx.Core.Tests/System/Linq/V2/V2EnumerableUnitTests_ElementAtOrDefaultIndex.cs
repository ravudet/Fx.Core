namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void ElementAtOrDefaultIndexMixinWithOverload()
        {
            var enumerable = new MockElementAtOrDefaultIndexMixinWithOverload().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexMixinWithOverload.Result;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton, elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexMixinWithOverload : IElementAtOrDefaultableMixin<object>
        {
            public static object? Result { get; } = new object().GetHashCode();

            public object? ElementAtOrDefault(Index index)
            {
                return (object?)Result;
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
        public void ElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton, elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsMixin : IElementAtOrDefaultableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IElementAtOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object? ElementAtOrDefault(Index index)
                {
                    return (object?)MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void ElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton.GetHashCode(), elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IElementAtOrDefaultableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockElementAtOrDefaultIndexMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void ElementAtOrDefaultIndexMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockElementAtOrDefaultIndexMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexMixinWithoutOverloadAndNoMonad.Element;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton.GetHashCode(), elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexMixinWithoutOverloadAndNoMonad : IElementAtOrDefaultableMixin<object>
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
        public void ElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsMixin.Result;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton, elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IElementAtOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object? ElementAtOrDefault(Index index)
                {
                    return (object?)MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void ElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton.GetHashCode(), elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void ElementAtOrDefaultIndexNoMixinAndNoMonad()
        {
            var enumerable = new MockElementAtOrDefaultIndexNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockElementAtOrDefaultIndexNoMixinAndNoMonad.Element;
            var elementatordefaulted = enumerable.ElementAtOrDefault(new Index(0));
            Assert.AreEqual<object?>(singleton.GetHashCode(), elementatordefaulted);
        }

        private sealed class MockElementAtOrDefaultIndexNoMixinAndNoMonad : IV2Enumerable<object>
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