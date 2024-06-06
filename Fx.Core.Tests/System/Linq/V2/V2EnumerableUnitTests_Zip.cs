namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// Zips a mixin that does implement the Zip overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void ZipMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockZipMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<string>(), monad.Source);
        }

        private sealed class MockZipMixinWithOverloadAndMonadWhereSourceIsNotMixin : IZipableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<(object First, TSecond Second)> Zip<TSecond>(IV2Enumerable<TSecond> second)
            {
                //// TODO
                return Result<TSecond>();
            }

            public static IV2Enumerable<(object, TSecond)> Result<TSecond>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond)>.Instance;
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
        /// Zips a mixin that does implement the Zip overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void ZipMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockZipMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            Assert.AreEqual(MockZipMixinWithOverloadAndNoMonad.Result<string>(), ziped);
        }

        private sealed class MockZipMixinWithOverloadAndNoMonad : IZipableMixin<object>
        {
            public IV2Enumerable<(object First, TSecond Second)> Zip<TSecond>(IV2Enumerable<TSecond> second)
            {
                //// TODO
                return Result<TSecond>();
            }

            public static IV2Enumerable<(object, TSecond)> Result<TSecond>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond)>.Instance;
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
        /// Zips a mixin that does not implement the Zip overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void ZipMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockZipMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockZipMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockZipMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<string>(), source.Source);
        }

        private sealed class MockZipMixinWithoutOverloadAndMonadWhereSourceIsMixin : IZipableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IZipableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<(object First, TSecond Second)> Zip<TSecond>(IV2Enumerable<TSecond> second)
                {
                    //// TODO
                    return Result<TSecond>();
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

            public static IV2Enumerable<(object, TSecond)> Result<TSecond>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond)>.Instance;
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
        /// Zips a mixin that does not implement the Zip overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void ZipMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(Array.Empty<(string, string, string)>(), monad.Source.ToArray());
        }

        private sealed class MockZipMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IZipableMixin<object>, IEnumerableMonad<object>
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
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// Zips a mixin that does not implement the Zip overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void ZipMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockZipMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            CollectionAssert.AreEqual(enumerable.AsEnumerable().Zip(new[] { string.Empty }.ToV2Enumerable()).ToArray(), ziped.ToArray());
        }

        private sealed class MockZipMixinWithoutOverloadAndNoMonad : IZipableMixin<object>
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
        /// Zips a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void ZipNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockZipNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipNoMixinAndMonadWhereSourceIsMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockZipNoMixinAndMonadWhereSourceIsMixin.Result<string>(), monad.Source);
        }

        private sealed class MockZipNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IZipableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IV2Enumerable<(object First, TSecond Second)> Zip<TSecond>(IV2Enumerable<TSecond> second)
                {
                    //// TODO
                    return Result<TSecond>();
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

            public static IV2Enumerable<(object, TSecond)> Result<TSecond>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond)>.Instance;
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
        /// Zips a monad where the source is a not mixin
        /// </summary>
        [TestMethod]
        public void ZipNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string)>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(enumerable.AsEnumerable().Zip(new[] { string.Empty }.ToV2Enumerable()).ToArray(), monad.Source.ToArray());
        }

        private sealed class MockZipNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        /// Zips a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void ZipNoMixinAndNoMonad()
        {
            var enumerable = new MockZipNoMixinAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable());
            CollectionAssert.AreEqual(enumerable.AsEnumerable().Zip(new[] { string.Empty }.ToV2Enumerable()).ToArray(), ziped.ToArray());
        }

        private sealed class MockZipNoMixinAndNoMonad : IV2Enumerable<object>
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