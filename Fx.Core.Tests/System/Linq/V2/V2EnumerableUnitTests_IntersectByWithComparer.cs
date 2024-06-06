namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// IntersectBys a mixin that does implement the IntersectByWithComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockIntersectByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            var monad = intersectbyed as MockIntersectByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockIntersectByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result(), monad.Source);
        }

        private sealed class MockIntersectByWithComparerMixinWithOverloadAndMonadWhereSourceIsNotMixin : IIntersectByableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> IntersectBy<TKey>(IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
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
        /// IntersectBys a mixin that does implement the IntersectByWithComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockIntersectByWithComparerMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            Assert.AreEqual(MockIntersectByWithComparerMixinWithOverloadAndNoMonad.Result(), intersectbyed);
        }

        private sealed class MockIntersectByWithComparerMixinWithOverloadAndNoMonad : IIntersectByableMixin<object>
        {
            public IV2Enumerable<object> IntersectBy<TKey>(IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
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
        /// IntersectBys a mixin that does not implement the IntersectByWithComparer overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            var monad = intersectbyed as MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result(), source.Source);
        }

        private sealed class MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsMixin : IIntersectByableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IIntersectByableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> IntersectBy<TKey>(IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
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
        /// IntersectBys a mixin that does not implement the IntersectByWithComparer overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            var monad = intersectbyed as MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().IntersectBy(V2Enumerable.Empty<object>(), element => element, null).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockIntersectByWithComparerMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IIntersectByableMixin<object>, IEnumerableMonad<object>
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
        /// IntersectBys a mixin that does not implement the IntersectByWithComparer overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockIntersectByWithComparerMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().IntersectBy(V2Enumerable.Empty<object>(), element => element, null).ToArray(), intersectbyed.ToArray());
        }

        private sealed class MockIntersectByWithComparerMixinWithoutOverloadAndNoMonad : IIntersectByableMixin<object>
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
        /// IntersectBys a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            var monad = intersectbyed as MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsMixin.Result(), monad.Source);
        }

        private sealed class MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IIntersectByableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<object> IntersectBy<TKey>(IV2Enumerable<TKey> second, Func<object, TKey> keySelector, IEqualityComparer<TKey>? comparer)
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
        /// IntersectBys a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            var monad = intersectbyed as MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<object>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().IntersectBy(V2Enumerable.Empty<object>(), element => element, null).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockIntersectByWithComparerNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        /// IntersectBys a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void IntersectByWithComparerNoMixinAndNoMonad()
        {
            var enumerable = new MockIntersectByWithComparerNoMixinAndNoMonad().AsV2Enumerable();
            var intersectbyed = enumerable.IntersectBy(V2Enumerable.Empty<object>(), element => element, null);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().IntersectBy(V2Enumerable.Empty<object>(), element => element, null).ToArray(), intersectbyed.ToArray());
        }

        private sealed class MockIntersectByWithComparerNoMixinAndNoMonad : IV2Enumerable<object>
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