namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithElementSelectorAndResultSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
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
        /// GroupBys a mixin that does implement the GroupByWithElementSelectorAndResultSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            Assert.AreEqual(MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndNoMonad.Result<object>(), groupbyed);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndResultSelectorAndComparer overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object>(), source.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

                public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndResultSelectorAndComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, (key, elements) => (object)this, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelectorAndResultSelectorAndComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, (key, elements) => (object)this, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerMixinWithoutOverloadAndNoMonad : IGroupByableMixin<object>
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
        public void GroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin.Result<object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public IV2Enumerable<TResult> GroupBy<TKey, TElement, TResult>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector, Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
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
        public void GroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            var monad = groupbyed as MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, (key, elements) => (object)this, null).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void GroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element, (key, elements) => (object)this, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element, (key, elements) => (object)this, null).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorAndResultSelectorAndComparerNoMixinAndNoMonad : IV2Enumerable<object>
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