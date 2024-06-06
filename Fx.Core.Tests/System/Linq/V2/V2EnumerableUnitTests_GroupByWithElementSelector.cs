namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// GroupBys a mixin that does implement the GroupByWithElementSelector overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            var monad = groupbyed as MockGroupByWithElementSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object, object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector)
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
        /// GroupBys a mixin that does implement the GroupByWithElementSelector overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            Assert.AreEqual(MockGroupByWithElementSelectorMixinWithOverloadAndNoMonad.Result<object, object>(), groupbyed);
        }

        private sealed class MockGroupByWithElementSelectorMixinWithOverloadAndNoMonad : IGroupByableMixin<object>
        {
            public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector)
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelector overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            var monad = groupbyed as MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object, object>(), source.Source);
        }

        private sealed class MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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

                public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector)
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelector overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            var monad = groupbyed as MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IGroupByableMixin<object>, IEnumerableMonad<object>
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
        /// GroupBys a mixin that does not implement the GroupByWithElementSelector overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void GroupByWithElementSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorMixinWithoutOverloadAndNoMonad : IGroupByableMixin<object>
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
        public void GroupByWithElementSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            var monad = groupbyed as MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsMixin.Result<object, object>(), monad.Source);
        }

        private sealed class MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TKey, TElement>(Func<object, TKey> keySelector, Func<object, TElement> elementSelector)
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
        public void GroupByWithElementSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            var monad = groupbyed as MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<IV2Grouping<object, object>>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element).ToArray(), monad.Source.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void GroupByWithElementSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockGroupByWithElementSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var groupbyed = enumerable.GroupBy(element => element, element => element);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().GroupBy(element => element, element => element).ToArray(), groupbyed.ToArray(), GroupingComparer.Instance);
        }

        private sealed class MockGroupByWithElementSelectorNoMixinAndNoMonad : IV2Enumerable<object>
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