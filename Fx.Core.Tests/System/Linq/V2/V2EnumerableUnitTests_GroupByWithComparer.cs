namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            var monad = groupbyed as MockGroupByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<IV2Grouping<TKey, object>> GroupBy<TKey>(Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TKey>();
            }

            public static IV2Enumerable<IV2Grouping<TKey, object>> Result<TKey>()
            {
                return ResultEnumerable<IV2Grouping<TKey, object>>.Instance;
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
        /// GroupBys a mixin that does implement the GroupByWithComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithComparerMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithComparerMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            Assert.AreEqual(MockGroupByWithComparerMixinWithOverloadAndNoMonad.Result<object>(), groupbyed);
        }

        private sealed class MockGroupByWithComparerMixinWithOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public IV2Enumerable<IV2Grouping<TKey, object>> GroupBy<TKey>(Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            {
                return Result<TKey>();
            }

            public static IV2Enumerable<IV2Grouping<TKey, object>> Result<TKey>()
            {
                return ResultEnumerable<IV2Grouping<TKey, object>>.Instance;
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
        /// GroupBys a mixin that does not implement the GroupByWithComparer overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            var monad = groupbyed as MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object>(), source.Source);
        }

        private sealed class MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

                public IV2Enumerable<IV2Grouping<TKey, object>> GroupBy<TKey>(Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TKey>();
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

            public static IV2Enumerable<IV2Grouping<TKey, object>> Result<TKey>()
            {
                return ResultEnumerable<IV2Grouping<TKey, object>>.Instance;
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
        /// GroupBys a mixin that does not implement the GroupByWithComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            var monad = groupbyed as MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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
        /// GroupBys a mixin that does not implement the GroupByWithComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithComparerMixinWithoutOverloadAndNoMonad : IGroupByableMixin<object>
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
        public void GroupByWithComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            var monad = groupbyed as MockGroupByWithComparerNoMixinAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithComparerNoMixinAndMonadWhereSourceIsMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public IV2Enumerable<IV2Grouping<TKey, object>> GroupBy<TKey>(Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
                {
                    return Result<TKey>();
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

            public static IV2Enumerable<IV2Grouping<TKey, object>> Result<TKey>()
            {
                return ResultEnumerable<IV2Grouping<TKey, object>>.Instance;
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
        public void GroupByWithComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            var monad = groupbyed as MockGroupByWithComparerNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void GroupByWithComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockGroupByWithComparerNoMixinAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithComparerNoMixinAndNoMonad : IV2Enumerable<object>
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