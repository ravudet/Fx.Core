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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithOverload()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithOverload().AsV2Enumerable();
            var singleton = MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithOverload.Result;
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            Assert.AreEqual(singleton, todictionaryed);
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithOverload : IToDictionaryableMixin<object>
        {
            public static Dictionary<object, object> Result { get; } = new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) });

            public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
            {
                return (Dictionary<TKey, TElement>)(IDictionary<TKey, TElement>)Result;
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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            Assert.AreEqual(singleton, todictionaryed);
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IToDictionaryableMixin<object>, IEnumerableMonad<object>
        {
            public static Dictionary<object, object> Result { get; } = new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) });

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

            private sealed class SourceEnumerable : IToDictionaryableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
                {
                    return (Dictionary<TKey, TElement>)(IDictionary<TKey, TElement>)MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            CollectionAssert.AreEqual(ToNonGenericCollection(new Dictionary<object, object>()), ToNonGenericCollection(todictionaryed));
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IToDictionaryableMixin<object>, IEnumerableMonad<object>
        {
            public static Dictionary<object, object> Element { get; } = new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) });

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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndNoMonad.Element;
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            CollectionAssert.AreEqual(ToNonGenericCollection(new Dictionary<object, object>()), ToNonGenericCollection(todictionaryed));
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerMixinWithoutOverloadAndNoMonad : IToDictionaryableMixin<object>
        {
            public static Dictionary<object, object> Element { get; } = new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) });

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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.Result;
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            Assert.AreEqual(singleton, todictionaryed);
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static Dictionary<object, object> Result { get; } = new Dictionary<object, object>(new[] { KeyValuePair.Create(new object(), new object()) });

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

            private sealed class SourceEnumerable : IToDictionaryableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull
                {
                    return (Dictionary<TKey, TElement>)(IDictionary<TKey, TElement>)MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            CollectionAssert.AreEqual(ToNonGenericCollection(new Dictionary<object, object>()), ToNonGenericCollection(todictionaryed));
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void ToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndNoMonad().AsV2Enumerable();
            var todictionaryed = enumerable.ToDictionary(_ => _, _ => _, EqualityComparer<object>.Default);
            CollectionAssert.AreEqual(ToNonGenericCollection(new Dictionary<object, object>()), ToNonGenericCollection(todictionaryed));
        }

        private sealed class MockToDictionaryWithKeySelectorElementSelectorAndComparerNoMixinAndNoMonad : IV2Enumerable<object>
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