namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// SkipWhiles a mixin that does implement the SkipWhileWithIndexPredicate overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            var monad = skipwhileed as MockSkipWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockSkipWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result(), monad.Source);
        }

        private sealed class MockSkipWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin : ISkipWhileableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> SkipWhile(Func<object, int, bool> predicate)
            {
                return Result();
            }

            public static IV2Enumerable<object> Result()
            {
                return ResultEnumerable<object>.Instance;
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
        /// SkipWhiles a mixin that does implement the SkipWhileWithIndexPredicate overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            Assert.AreEqual(MockSkipWhileWithIndexPredicateMixinWithOverloadAndNoMonad.Result(), skipwhileed);
        }

        private sealed class MockSkipWhileWithIndexPredicateMixinWithOverloadAndNoMonad : ISkipWhileableMixin<object>
        {
            public IV2Enumerable<object> SkipWhile(Func<object, int, bool> predicate)
            {
                return Result();
            }

            public static IV2Enumerable<object> Result()
            {
                return ResultEnumerable<object>.Instance;
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
        /// SkipWhiles a mixin that does not implement the SkipWhileWithIndexPredicate overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            var monad = skipwhileed as MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result(), source.Source);
        }

        private sealed class MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISkipWhileableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISkipWhileableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> SkipWhile(Func<object, int, bool> predicate)
                {
                    return Result();
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

            public static IV2Enumerable<object> Result()
            {
                return ResultEnumerable<object>.Instance;
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
        /// SkipWhiles a mixin that does not implement the SkipWhileWithIndexPredicate overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            var monad = skipwhileed as MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SkipWhile((element, index) => true).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISkipWhileableMixin<object>, IEnumerableMonad<object>
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
        /// SkipWhiles a mixin that does not implement the SkipWhileWithIndexPredicate overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SkipWhile((element, index) => true).ToArray(), skipwhileed.ToArray());
        }

        private sealed class MockSkipWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad : ISkipWhileableMixin<object>
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
        /// SkipWhiles a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            var monad = skipwhileed as MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin.Result(), monad.Source);
        }

        private sealed class MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ISkipWhileableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> SkipWhile(Func<object, int, bool> predicate)
                {
                    return Result();
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

            public static IV2Enumerable<object> Result()
            {
                return ResultEnumerable<object>.Instance;
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
        /// SkipWhiles a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            var monad = skipwhileed as MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SkipWhile((element, index) => true).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockSkipWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        /// SkipWhiles a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void SkipWhileWithIndexPredicateNoMixinAndNoMonad()
        {
            var enumerable = new MockSkipWhileWithIndexPredicateNoMixinAndNoMonad().AsV2Enumerable();
            var skipwhileed = enumerable.SkipWhile((element, index) => true);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().SkipWhile((element, index) => true).ToArray(), skipwhileed.ToArray());
        }

        private sealed class MockSkipWhileWithIndexPredicateNoMixinAndNoMonad : IV2Enumerable<object>
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