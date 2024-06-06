namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithElementSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object, object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = V2Enumerable.Empty<object>();

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

            public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TKey, TElement>();
            }

            public static IV2Enumerable<IV2Grouping<TKey, TElement>> Result<TKey, TElement>()
            {
                return ResultEnumerable<IV2Grouping<TKey, TElement>>.Instance;
            }

            private sealed class ResultEnumerable<T> : IV2Enumerable<T>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<T> Instance { get; } = new ResultEnumerable<T>();

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

        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithElementSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            Assert.AreEqual(MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndNoMonad.Result<object, object>(), groupbyed);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerMixinWithOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TKey, TElement>();
            }

            public static IV2Enumerable<IV2Grouping<TKey, TElement>> Result<TKey, TElement>()
            {
                return ResultEnumerable<IV2Grouping<TKey, TElement>>.Instance;
            }

            private sealed class ResultEnumerable<T> : IV2Enumerable<T>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<T> Instance { get; } = new ResultEnumerable<T>();

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

        /// <summary>
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndComparer overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object, object>(), source.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IGroupByableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TKey, TElement>();
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

            public static IV2Enumerable<IV2Grouping<TKey, TElement>> Result<TKey, TElement>()
            {
                return ResultEnumerable<IV2Grouping<TKey, TElement>>.Instance;
            }

            private sealed class ResultEnumerable<T> : IV2Enumerable<T>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<T> Instance { get; } = new ResultEnumerable<T>();

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
        
        /// <summary>
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = V2Enumerable.Empty<string>();

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
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerMixinWithoutOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public static object ResultObject { get; } = new object();

            public IEnumerator<object> GetEnumerator()
            {
                yield return ResultObject;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// GroupBys a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.Result<object, object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IGroupByableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TKey, TElement>();
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

            public static IV2Enumerable<IV2Grouping<TKey, TElement>> Result<TKey, TElement>()
            {
                return ResultEnumerable<IV2Grouping<TKey, TElement>>.Instance;
            }

            private sealed class ResultEnumerable<T> : IV2Enumerable<T>
            {
                private ResultEnumerable()
                {
                }

                public static ResultEnumerable<T> Instance { get; } = new ResultEnumerable<T>();

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
        
        /// <summary>
        /// GroupBys a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object ResultObject { get; } = new object();

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    yield return ResultObject;
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
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// GroupBys a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndComparerNoMixinAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndComparerNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object ResultObject { get; } = new object();

            public IEnumerator<object> GetEnumerator()
            {
                yield return ResultObject;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}