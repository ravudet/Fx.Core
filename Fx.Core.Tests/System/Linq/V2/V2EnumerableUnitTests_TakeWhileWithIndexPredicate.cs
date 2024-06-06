namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// TakeWhiles a mixin that does implement the TakeWhileWithIndexPredicate overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            var monad = takewhileed as MockTakeWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockTakeWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result(), monad.Source);
        }

        private sealed class MockTakeWhileWithIndexPredicateMixinWithOverloadAndMonadWhereSourceIsNotMixin : ITakeWhileableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> TakeWhile(Func<object, int, bool> predicate)
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
        /// TakeWhiles a mixin that does implement the TakeWhileWithIndexPredicate overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            Assert.AreEqual(MockTakeWhileWithIndexPredicateMixinWithOverloadAndNoMonad.Result(), takewhileed);
        }

        private sealed class MockTakeWhileWithIndexPredicateMixinWithOverloadAndNoMonad : ITakeWhileableMixin<object>
        {
            public IV2Enumerable<object> TakeWhile(Func<object, int, bool> predicate)
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
        /// TakeWhiles a mixin that does not implement the TakeWhileWithIndexPredicate overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            var monad = takewhileed as MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result(), source.Source);
        }

        private sealed class MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin : ITakeWhileableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ITakeWhileableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> TakeWhile(Func<object, int, bool> predicate)
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
        /// TakeWhiles a mixin that does not implement the TakeWhileWithIndexPredicate overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            var monad = takewhileed as MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().TakeWhile((element, index) => true).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ITakeWhileableMixin<object>, IEnumerableMonad<object>
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
        /// TakeWhiles a mixin that does not implement the TakeWhileWithIndexPredicate overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().TakeWhile((element, index) => true).ToArray(), takewhileed.ToArray());
        }

        private sealed class MockTakeWhileWithIndexPredicateMixinWithoutOverloadAndNoMonad : ITakeWhileableMixin<object>
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
        /// TakeWhiles a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            var monad = takewhileed as MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin.Result(), monad.Source);
        }

        private sealed class MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : ITakeWhileableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> TakeWhile(Func<object, int, bool> predicate)
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
        /// TakeWhiles a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            var monad = takewhileed as MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().TakeWhile((element, index) => true).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockTakeWhileWithIndexPredicateNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        /// TakeWhiles a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void TakeWhileWithIndexPredicateNoMixinAndNoMonad()
        {
            var enumerable = new MockTakeWhileWithIndexPredicateNoMixinAndNoMonad().AsV2Enumerable();
            var takewhileed = enumerable.TakeWhile((element, index) => true);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().TakeWhile((element, index) => true).ToArray(), takewhileed.ToArray());
        }

        private sealed class MockTakeWhileWithIndexPredicateNoMixinAndNoMonad : IV2Enumerable<object>
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