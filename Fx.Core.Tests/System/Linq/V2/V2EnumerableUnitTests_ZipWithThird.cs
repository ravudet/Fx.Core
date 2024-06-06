namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        /// <summary>
        /// Zips a mixin that does implement the ZipWithThird overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void ZipWithThirdMixinWithOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipWithThirdMixinWithOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipWithThirdMixinWithOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockZipWithThirdMixinWithOverloadAndMonadWhereSourceIsNotMixin.Result<string, string>(), monad.Source);
        }

        private sealed class MockZipWithThirdMixinWithOverloadAndMonadWhereSourceIsNotMixin : IZipableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<(object First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
                IV2Enumerable<TSecond> second,
                IV2Enumerable<TThird> third)
            {
                //// TODO
                return Result<TSecond, TThird>();
            }

            public static IV2Enumerable<(object, TSecond, TThird)> Result<TSecond, TThird>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond, TThird)>.Instance;
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
        /// Zips a mixin that does implement the ZipWithThird overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void ZipWithThirdMixinWithOverloadAndNoMonad()
        {
            var enumerable = new MockZipWithThirdMixinWithOverloadAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            Assert.AreEqual(MockZipWithThirdMixinWithOverloadAndNoMonad.Result<string, string>(), ziped);
        }

        private sealed class MockZipWithThirdMixinWithOverloadAndNoMonad : IZipableMixin<object>
        {
            public IV2Enumerable<(object First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
                IV2Enumerable<TSecond> second,
                IV2Enumerable<TThird> third)
            {
                //// TODO
                return Result<TSecond, TThird>();
            }

            public static IV2Enumerable<(object, TSecond, TThird)> Result<TSecond, TThird>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond, TThird)>.Instance;
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
        /// Zips a mixin that does not implement the ZipWithThird overload and does implement a monad where the source is a mixin
        /// </summary>
        [TestMethod]
        public void ZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(monad);
            var source = monad.Source as MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(source);
            Assert.AreEqual(MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result<string, string>(), source.Source);
        }

        private sealed class MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsMixin : IZipableMixin<object>, IEnumerableMonad<object>
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

                public IV2Enumerable<(object First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
                    IV2Enumerable<TSecond> second,
                    IV2Enumerable<TThird> third)
                {
                    //// TODO
                    return Result<TSecond, TThird>();
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

            public static IV2Enumerable<(object, TSecond, TThird)> Result<TSecond, TThird>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond, TThird)>.Instance;
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
        /// Zips a mixin that does not implement the ZipWithThird overload and does implement a monad where the source is not a mixin
        /// </summary>
        [TestMethod]
        public void ZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(Array.Empty<(string, string, string)>(), monad.Source.ToArray());
        }

        private sealed class MockZipWithThirdMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IZipableMixin<object>, IEnumerableMonad<object>
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
        /// Zips a mixin that does not implement the ZipWithThird overload and does not implement a monad
        /// </summary>
        [TestMethod]
        public void ZipWithThirdMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockZipWithThirdMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            CollectionAssert.AreEqual(new[] { (MockZipWithThirdMixinWithoutOverloadAndNoMonad.ResultObject, string.Empty, string.Empty) }, ziped.ToArray());
        }

        private sealed class MockZipWithThirdMixinWithoutOverloadAndNoMonad : IZipableMixin<object>
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
        public void ZipWithThirdNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockZipWithThirdNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipWithThirdNoMixinAndMonadWhereSourceIsMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(monad);
            Assert.AreEqual(MockZipWithThirdNoMixinAndMonadWhereSourceIsMixin.Result<string, string>(), monad.Source);
        }

        private sealed class MockZipWithThirdNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

                public IV2Enumerable<(object First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
                    IV2Enumerable<TSecond> second,
                    IV2Enumerable<TThird> third)
                {
                    //// TODO
                    return Result<TSecond, TThird>();
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

            public static IV2Enumerable<(object, TSecond, TThird)> Result<TSecond, TThird>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond, TThird)>.Instance;
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
        public void ZipWithThirdNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockZipWithThirdNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            var monad = ziped as MockZipWithThirdNoMixinAndMonadWhereSourceIsNotMixin.ResultMonad<(object, string, string)>;
            Assert.IsNotNull(monad);
            CollectionAssert.AreEqual(new[] { (MockZipWithThirdNoMixinAndMonadWhereSourceIsNotMixin.ResultObject, string.Empty, string.Empty) }, monad.Source.ToArray());
        }

        private sealed class MockZipWithThirdNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static IV2Enumerable<(object, TSecond, TThird)> Result<TSecond, TThird>()
            {
                //// TODO
                return ResultEnumerable<(object, TSecond, TThird)>.Instance;
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
        /// Zips a <see cref="IV2Enumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void ZipWithThirdNoMixinAndNoMonad()
        {
            var enumerable = new MockZipWithThirdNoMixinAndNoMonad().AsV2Enumerable();
            var ziped = enumerable.Zip(new[] { string.Empty }.ToV2Enumerable(), new[] { string.Empty }.ToV2Enumerable());
            CollectionAssert.AreEqual(new[] { (MockZipWithThirdNoMixinAndNoMonad.ResultObject, string.Empty, string.Empty) }, ziped.ToArray());
        }

        private sealed class MockZipWithThirdNoMixinAndNoMonad : IV2Enumerable<object>
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