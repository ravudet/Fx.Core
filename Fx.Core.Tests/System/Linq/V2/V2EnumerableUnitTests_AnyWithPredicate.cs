namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public sealed partial class V2EnumerableUnitTests
    {
        [TestMethod]
        public void AnyWithPredicateMixinWithOverload()
        {
            var enumerable = new MockAnyWithPredicateMixinWithOverload().AsV2Enumerable();
            var singleton = MockAnyWithPredicateMixinWithOverload.Result;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton, anyed);
        }

        private sealed class MockAnyWithPredicateMixinWithOverload : IAnyableMixin<object>
        {
            public static BoolAdapter Result { get; } = new object().GetHashCode();

            public bool Any(Func<object, bool> predicate)
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
        public void AnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton, anyed);
        }

        private sealed class MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin : IAnyableMixin<object>, IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IAnyableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool Any(Func<object, bool> predicate)
                {
                    return (bool)MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsMixin.Result;
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
        public void AnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), anyed);
        }

        private sealed class MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin : IAnyableMixin<object>, IEnumerableMonad<object>
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

            public IV2Enumerable<object> Source { get; } = Enumerable.Repeat(MockAnyWithPredicateMixinWithoutOverloadAndMonadWhereSourceIsNotMixin.Element, 1).ToV2Enumerable();

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
        public void AnyWithPredicateMixinWithoutOverloadAndNoMonad()
        {
            var enumerable = new MockAnyWithPredicateMixinWithoutOverloadAndNoMonad().AsV2Enumerable();
            var singleton = MockAnyWithPredicateMixinWithoutOverloadAndNoMonad.Element;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), anyed);
        }

        private sealed class MockAnyWithPredicateMixinWithoutOverloadAndNoMonad : IAnyableMixin<object>
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
        public void AnyWithPredicateNoMixinAndMonadWhereSourceIsMixin()
        {
            var enumerable = new MockAnyWithPredicateNoMixinAndMonadWhereSourceIsMixin().AsV2Enumerable();
            var singleton = MockAnyWithPredicateNoMixinAndMonadWhereSourceIsMixin.Result;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton, anyed);
        }

        private sealed class MockAnyWithPredicateNoMixinAndMonadWhereSourceIsMixin : IEnumerableMonad<object>
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

            private sealed class SourceEnumerable : IAnyableMixin<object>
            {
                private SourceEnumerable()
                {
                }

                public static SourceEnumerable Instance { get; } = new SourceEnumerable();

                public bool Any(Func<object, bool> predicate)
                {
                    return (bool)MockAnyWithPredicateNoMixinAndMonadWhereSourceIsMixin.Result;
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
        public void AnyWithPredicateNoMixinAndMonadWhereSourceIsNotMixin()
        {
            var enumerable = new MockAnyWithPredicateNoMixinAndMonadWhereSourceIsNotMixin().AsV2Enumerable();
            var singleton = MockAnyWithPredicateNoMixinAndMonadWhereSourceIsNotMixin.Element;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), anyed);
        }

        private sealed class MockAnyWithPredicateNoMixinAndMonadWhereSourceIsNotMixin : IEnumerableMonad<object>
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
        public void AnyWithPredicateNoMixinAndNoMonad()
        {
            var enumerable = new MockAnyWithPredicateNoMixinAndNoMonad().AsV2Enumerable();
            var singleton = MockAnyWithPredicateNoMixinAndNoMonad.Element;
            var anyed = enumerable.Any(element => (BoolAdapter)(singleton.GetHashCode()));
            Assert.AreEqual<BoolAdapter>(singleton.GetHashCode(), anyed);
        }

        private sealed class MockAnyWithPredicateNoMixinAndNoMonad : IV2Enumerable<object>
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