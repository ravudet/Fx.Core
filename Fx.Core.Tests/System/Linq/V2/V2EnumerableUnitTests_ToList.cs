namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
/*
dimension under test:
1. mixin vs not a mixin
2. mixin overload is implemented vs mixin overload is not implement
3. monad is implemented vs monad is not implemented
4. monad source is mixin vs monad source is not mixin

mixin   overload    monad   sourcemixin     why the test sku doesn't make sense                         test name
T       T           T       T               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           T       F               if there's an overload, the monad won't get called and the result is terminal so it's not wrappable in the monad
T       T           F       T               if there's no monad, there's won't be a sourcemixin
T       T           F       F                                                                           MixinWithOverload
T       F           T       T                                                                           MixinWithoutOverloadAndMonadWhereSourceIsMixin
T       F           T       F                                                                           MixinWithoutOverloadAndMonadWhereSourceIsNotMixin
T       F           F       T               if there's no monad, there's won't be a sourcemixin
T       F           F       F                                                                           MixinWithoutOverloadAndNoMonad
F       T           T       T               if there's no mixin, there can't be an overload
F       T           T       F               if there's no mixin, there can't be an overload
F       T           F       T               if there's no mixin, there can't be an overload
F       T           F       F               if there's no mixin, there can't be an overload
F       F           T       T                                                                           NoMixinAndMonadWhereSourceIsMixin
F       F           T       F                                                                           NoMixinAndMonadWhereSourceIsNotMixin
F       F           F       T               if there's no monad, there's won't be a sourcemixin
F       F           F       F                                                                           NoMixinAndNoMonad
*/

        [TestMethod]
        public void ToListMixinWithOverload()
        {
            var enumerable = new MockToListMixinWithOverload().AsV2Enumerable();
            var singleton = MockToListMixinWithOverload.Result;
            var tolisted = enumerable.ToList();
            Assert.AreEqual(singleton, tolisted);
        }

        private sealed class MockToListMixinWithOverload : IToListableMixin<object>
        {
            public static List<object> Result { get; } = new List<object>(new[] { new object(), new object() });

            public List<object> ToList()
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
        public void ToListMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToListMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToListMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var tolisted = enumerable.ToList();
            Assert.AreEqual(singleton, tolisted);
        }

        private sealed class MockToListMixinWithoutOverloadAndMonadWhereSourceIsMixin : IToListableMixin<object>, IEnumerableMonad<object>
        {
            public static List<object> Result { get; } = new List<object>(new[] { new object(), new object() });

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

            private sealed class SourceEnumerable : IToListableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public List<object> ToList()
                {
                    return MockToListMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void ToListMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToListMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockToListMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var tolisted = enumerable.ToList();
            CollectionAssert.AreEqual(ToNonGenericCollection(new List<object>()), ToNonGenericCollection(tolisted));
        }

        private sealed class MockToListMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IToListableMixin<object>, IEnumerableMonad<object>
        {
            public static List<object> Element { get; } = new List<object>(new[] { new object(), new object() });

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Empty<object>().ToV2Enumerable();

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
        public void ToListMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockToListMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockToListMixinWithoutOverloadAndNoMonad.Element;
            var tolisted = enumerable.ToList();
            CollectionAssert.AreEqual(ToNonGenericCollection(new List<object>()), ToNonGenericCollection(tolisted));
        }

        private sealed class MockToListMixinWithoutOverloadAndNoMonad : IToListableMixin<object>
        {
            public static List<object> Element { get; } = new List<object>(new[] { new object(), new object() });

            public IEnumerator<object> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void ToListNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToListNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToListNoMixinAndMonadWhereSourceIsMixin.Result;
            var tolisted = enumerable.ToList();
            Assert.AreEqual(singleton, tolisted);
        }

        private sealed class MockToListNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static List<object> Result { get; } = new List<object>(new[] { new object(), new object() });

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

            private sealed class SourceEnumerable : IToListableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public List<object> ToList()
                {
                    return (List<object>)MockToListNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void ToListNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToListNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var tolisted = enumerable.ToList();
            CollectionAssert.AreEqual(ToNonGenericCollection(new List<object>()), ToNonGenericCollection(tolisted));
        }

        private sealed class MockToListNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    yield break;
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
        public void ToListNoMixinAndNoMonad()
        {
            var enumerable = new MockToListNoMixinAndNoMonad().AsV2Enumerable();
            var tolisted = enumerable.ToList();
            CollectionAssert.AreEqual(ToNonGenericCollection(new List<object>()), ToNonGenericCollection(tolisted));
        }

        private sealed class MockToListNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public IEnumerator<object> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}