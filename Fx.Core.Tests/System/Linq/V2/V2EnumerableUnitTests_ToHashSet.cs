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
        public void ToHashSetMixinWithOverload()
        {
            var enumerable = new MockToHashSetMixinWithOverload().AsV2Enumerable();
            var singleton = MockToHashSetMixinWithOverload.Result;
            var tohashseted = enumerable.ToHashSet();
            Assert.AreEqual(singleton, tohashseted);
        }

        private sealed class MockToHashSetMixinWithOverload : IToHashSetableMixin<object>
        {
            public static HashSet<object> Result { get; } = new HashSet<object>(new[] { new object(), new object() });

            public HashSet<object> ToHashSet()
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
        public void ToHashSetMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var tohashseted = enumerable.ToHashSet();
            Assert.AreEqual(singleton, tohashseted);
        }

        private sealed class MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsMixin : IToHashSetableMixin<object>, IEnumerableMonad<object>
        {
            public static HashSet<object> Result { get; } = new HashSet<object>(new[] { new object(), new object() });

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

            private sealed class SourceEnumerable : IToHashSetableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public HashSet<object> ToHashSet()
                {
                    return MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void ToHashSetMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var tohashseted = enumerable.ToHashSet();
            CollectionAssert.AreEqual(ToNonGenericCollection(new HashSet<object>()), ToNonGenericCollection(tohashseted));
        }

        private sealed class MockToHashSetMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IToHashSetableMixin<object>, IEnumerableMonad<object>
        {
            public static HashSet<object> Element { get; } = new HashSet<object>(new[] { new object(), new object() });

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
        public void ToHashSetMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockToHashSetMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockToHashSetMixinWithoutOverloadAndNoMonad.Element;
            var tohashseted = enumerable.ToHashSet();
            CollectionAssert.AreEqual(ToNonGenericCollection(new HashSet<object>()), ToNonGenericCollection(tohashseted));
        }

        private sealed class MockToHashSetMixinWithoutOverloadAndNoMonad : IToHashSetableMixin<object>
        {
            public static HashSet<object> Element { get; } = new HashSet<object>(new[] { new object(), new object() });

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
        public void ToHashSetNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToHashSetNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToHashSetNoMixinAndMonadWhereSourceIsMixin.Result;
            var tohashseted = enumerable.ToHashSet();
            Assert.AreEqual(singleton, tohashseted);
        }

        private sealed class MockToHashSetNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static HashSet<object> Result { get; } = new HashSet<object>(new[] { new object(), new object() });

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

            private sealed class SourceEnumerable : IToHashSetableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public HashSet<object> ToHashSet()
                {
                    return (HashSet<object>)MockToHashSetNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void ToHashSetNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToHashSetNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var tohashseted = enumerable.ToHashSet();
            CollectionAssert.AreEqual(ToNonGenericCollection(new HashSet<object>()), ToNonGenericCollection(tohashseted));
        }

        private sealed class MockToHashSetNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void ToHashSetNoMixinAndNoMonad()
        {
            var enumerable = new MockToHashSetNoMixinAndNoMonad().AsV2Enumerable();
            var tohashseted = enumerable.ToHashSet();
            CollectionAssert.AreEqual(ToNonGenericCollection(new HashSet<object>()), ToNonGenericCollection(tohashseted));
        }

        private sealed class MockToHashSetNoMixinAndNoMonad : IV2Enumerable<object>
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