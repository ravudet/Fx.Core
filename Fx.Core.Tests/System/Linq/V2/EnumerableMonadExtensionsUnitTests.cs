namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;

    [TestClass]
    public sealed class EnumerableMonadExtensionsUnitTests
    {
        [TestMethod]
        public void CreateMonad()
        {
            var monad = new AnyFalseMonad<string>(new[] { "asfd" }.ToV2Enumerable());
            var data = monad.Create(new[] { "qwer" }.ToV2Enumerable());
            Assert.IsFalse(data.Any());
        }

        private sealed class AnyFalseMonad<T> : IEnumerableMonad<T>, IAnyableMixin<T>
        {
            public AnyFalseMonad(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return enumerable => new AnyFalseMonad<TSource>(enumerable);
            }

            public bool Any()
            {
                return false;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }

        [TestMethod]
        public void CreatePassthroughMonad()
        {
            var monad = new CountNegative<string>(new AnyFalseMonad<string>(new[] { "asfd" }.ToV2Enumerable()));
            var data = monad.Create(new[] { "qwer" }.ToV2Enumerable());
            Assert.IsFalse(data.Any());
            Assert.AreEqual(-1, data.Count());
        }

        private sealed class CountNegative<T> : IEnumerableMonad<T>, ICountableMixin<T>
        {
            public CountNegative(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return enumerable => new CountNegative<TSource>(enumerable);
            }

            public int Count()
            {
                return -1;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.Source).GetEnumerator();
            }
        }
    }
}
