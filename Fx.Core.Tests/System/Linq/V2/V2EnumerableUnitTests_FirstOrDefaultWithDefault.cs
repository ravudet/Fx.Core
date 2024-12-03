namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void FirstOrDefaultWithDefaultMixinWithOverload()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultMixinWithOverload().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultMixinWithOverload.Result;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual((object)singleton, (object)firstordefaulted);
        }

        private sealed class MockFirstOrDefaultWithDefaultMixinWithOverload : IFirstOrDefaultableMixin<object>
        {
            public static object Result { get; } = new object().GetHashCode();

            public object FirstOrDefault(object defaultValue)
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
        public void FirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual((object)singleton, (object)firstordefaulted);
        }

        private sealed class MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin : IFirstOrDefaultableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IFirstOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object FirstOrDefault(object defaultValue)
                {
                    return (object)MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void FirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual<object>(singleton.GetHashCode(), firstordefaulted.GetHashCode());
        }

        private sealed class MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IFirstOrDefaultableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = new[] { MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element }.ToV2Enumerable();

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
        public void FirstOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad.Element;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual(singleton.GetHashCode(), firstordefaulted.GetHashCode());
        }

        private sealed class MockFirstOrDefaultWithDefaultMixinWithoutOverloadAndNoMonad : IFirstOrDefaultableMixin<object>
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
        public void FirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual((object)singleton, (object)firstordefaulted);
        }

        private sealed class MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IFirstOrDefaultableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public object FirstOrDefault(object defaultValue)
                {
                    return (object)MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void FirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual(singleton.GetHashCode(), firstordefaulted.GetHashCode());
        }

        private sealed class MockFirstOrDefaultWithDefaultNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void FirstOrDefaultWithDefaultNoMixinAndNoMonad()
        {
            var enumerable = new MockFirstOrDefaultWithDefaultNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockFirstOrDefaultWithDefaultNoMixinAndNoMonad.Element;
            var firstordefaulted = enumerable.FirstOrDefault(singleton);
            Assert.AreEqual(singleton.GetHashCode(), firstordefaulted.GetHashCode());
        }

        private sealed class MockFirstOrDefaultWithDefaultNoMixinAndNoMonad : IV2Enumerable<object>
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