namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void LastOrDefaultWithDefaultMixinWithOverload()
        {
            var enumerable = new MockLastOrDefaultWithDefaultMixinWithOverload().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultMixinWithOverload.Result;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithDefaultMixinWithOverload : ILastOrDefaultableMixin<object>
        {
            public static object Result { get; } = new object().GetHashCode();

            public object LastOrDefault(object defaultValue)
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
        public void LastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin : ILastOrDefaultableMixin<object>, IEnumerableMonad<object>
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

                public object LastOrDefault(object defaultValue)
                {
                    return (object)MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void LastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted.GetHashCode());
        }

        private sealed class MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ILastOrDefaultableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = new[] { MockLastOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element }.ToV2Enumerable();

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
        public void LastOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockLastOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad.Element;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted.GetHashCode());
        }

        private sealed class MockLastOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad : ILastOrDefaultableMixin<object>
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
        public void LastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton, lastordefaulted);
        }

        private sealed class MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public object LastOrDefault(object defaultValue)
                {
                    return (object)MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void LastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted.GetHashCode());
        }

        private sealed class MockLastOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void LastOrDefaultWithDefaultNoMixinAndNoMonad()
        {
            var enumerable = new MockLastOrDefaultWithDefaultNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockLastOrDefaultWithDefaultNoMixinAndNoMonad.Element;
            var lastordefaulted = enumerable.LastOrDefault(singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), lastordefaulted.GetHashCode());
        }

        private sealed class MockLastOrDefaultWithDefaultNoMixinAndNoMonad : IV2Enumerable<object>
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