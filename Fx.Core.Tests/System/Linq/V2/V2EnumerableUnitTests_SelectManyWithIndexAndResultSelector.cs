namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// SelectManys a mixin that does implement the SelectManyWithIndexAndResultSelector overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            var monad = selectmanyed as MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<object>(), monad.Source);
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndMonadWhereSourceIsNotMixin : ISelectManyableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(Func<object, int, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector)
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
        /// SelectManys a mixin that does implement the SelectManyWithIndexAndResultSelector overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            Assert.AreEqual(MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndNoMonad.Result<object>(), selectmanyed);
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorMixinWithOverloadAndNoMonad : ISelectManyableMixin<object>
        {
            public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(Func<object, int, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector)
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
        /// SelectManys a mixin that does not implement the SelectManyWithIndexAndResultSelector overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            var monad = selectmanyed as MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<object>(), source.Source);
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISelectManyableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISelectManyableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(Func<object, int, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector)
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
        /// SelectManys a mixin that does not implement the SelectManyWithIndexAndResultSelector overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            var monad = selectmanyed as MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISelectManyableMixin<object>, IEnumerableMonad<object>
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
        /// SelectManys a mixin that does not implement the SelectManyWithIndexAndResultSelector overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection).ToArray(), selectmanyed.ToArray());
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorMixinWithoutOverloadAndNoMonad : ISelectManyableMixin<object>
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
        /// SelectManys a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            var monad = selectmanyed as MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsMixin.Result<object>(), monad.Source);
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISelectManyableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(Func<object, int, IV2Enumerable<TCollection>> collectionSelector, Func<object, TCollection, TResult> resultSelector)
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
        /// SelectManys a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            var monad = selectmanyed as MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        /// SelectManys a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void SelectManyWithIndexAndResultSelectorNoMixinAndNoMonad()
        {
            var enumerable = new MockSelectManyWithIndexAndResultSelectorNoMixinAndNoMonad().AsV2Enumerable();
            var selectmanyed = enumerable.SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SelectMany((element, index) => V2Enumerable.Empty<object>(), (element, collection) => collection).ToArray(), selectmanyed.ToArray());
        }

        private sealed class MockSelectManyWithIndexAndResultSelectorNoMixinAndNoMonad : IV2Enumerable<object>
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