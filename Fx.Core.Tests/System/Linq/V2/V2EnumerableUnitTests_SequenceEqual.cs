namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void SequenceEqualMixinWithOverload()
        {
            var enumerable = new MockSequenceEqualMixinWithOverload().AsV2Enumerable();
            var singleton = MockSequenceEqualMixinWithOverload.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualMixinWithOverload : ISequenceEqualableMixin<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

            public bool SequenceEqual(IV2Enumerable<object> second)
            {
                return (bool)Result;
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

        [TestMethod]
        public void SequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsMixin : ISequenceEqualableMixin<object>, IEnumerableMonad<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ISequenceEqualableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool SequenceEqual(IV2Enumerable<object> second)
                {
                    return (bool)MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : ISequenceEqualableMixin<object>, IEnumerableMonad<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode();

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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockSequenceEqualMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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

        [TestMethod]
        public void SequenceEqualMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockSequenceEqualMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockSequenceEqualMixinWithoutOverloadAndNoMonad.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualMixinWithoutOverloadAndNoMonad : ISequenceEqualableMixin<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < 1; ++i)
                {
                    yield return Element;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockSequenceEqualNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualNoMixinAndMonadWhereSourceIsMixin.Result;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton, sequenceequaled);
        }

        private sealed class MockSequenceEqualNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

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

            private sealed class SourceEnumerable : ISequenceEqualableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool SequenceEqual(IV2Enumerable<object> second)
                {
                    return (bool)MockSequenceEqualNoMixinAndMonadWhereSourceIsMixin.Result;
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

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockSequenceEqualNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockSequenceEqualNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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

            public static object Element { get; } = (BoolAdapter)new object().GetHashCode();

            private sealed class SourceEnumerable : IV2Enumerable<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public IEnumerator<object> GetEnumerator()
                {
                    for (int i = 0; i < 1; ++i)
                    {
                        yield return Element;
                    }
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
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqualNoMixinAndNoMonad()
        {
            var enumerable = new MockSequenceEqualNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockSequenceEqualNoMixinAndNoMonad.Element;
            var sequenceequaled = enumerable.SequenceEqual(new[] { BoolAdapter.True }.ToV2Enumerable());
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), sequenceequaled);
        }

        private sealed class MockSequenceEqualNoMixinAndNoMonad : IV2Enumerable<object>
        {
            public static object Element { get; } = (BoolAdapter)new object().GetHashCode();

            public IEnumerator<object> GetEnumerator()
            {
                for (int i = 0; i < 1; ++i)
                {
                    yield return Element;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}