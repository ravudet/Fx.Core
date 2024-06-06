namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithResultSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<TResult> GroupBy<TKey, TResult>(Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TResult>();
            }

            public static IV2Enumerable<TResult> Result<TResult>()
            {
                return ResultEnumerable<TResult>.Instance;
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
        /// GroupBys a mixin that does implement the GroupByWithResultSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithResultSelectorAndComparerMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            Assert.AreEqual(MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndNoMonad.Result<object>(), groupbyed);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerMixinWithOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public IV2Enumerable<TResult> GroupBy<TKey, TResult>(Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TResult>();
            }

            public static IV2Enumerable<TResult> Result<TResult>()
            {
                return ResultEnumerable<TResult>.Instance;
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
        /// GroupBys a mixin that does not implement the GroupByWithResultSelectorAndComparer overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object>(), source.Source);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

                public IV2Enumerable<TResult> GroupBy<TKey, TResult>(Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TResult>();
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

            public static IV2Enumerable<TResult> Result<TResult>()
            {
                return ResultEnumerable<TResult>.Instance;
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
        /// GroupBys a mixin that does not implement the GroupByWithResultSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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
        /// GroupBys a mixin that does not implement the GroupByWithResultSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithResultSelectorAndComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerMixinWithoutOverloadAndNoMonad : IGroupByableMixin<object>
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
        public void GroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public IV2Enumerable<TResult> GroupBy<TKey, TResult>(Func<object, TKey> keySelector, Func<TKey, IV2Enumerable<object>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TResult>();
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

            public static IV2Enumerable<TResult> Result<TResult>()
            {
                return ResultEnumerable<TResult>.Instance;
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
        public void GroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            var monad = groupbyed as MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void GroupByWithResultSelectorAndComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockGroupByWithResultSelectorAndComparerNoMixinAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithResultSelectorAndComparerNoMixinAndNoMonad : IV2Enumerable<object>
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