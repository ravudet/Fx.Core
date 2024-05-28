namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;

    [TestClass]
    public sealed class ScenarioTests
    {
        [TestMethod]
        public void Test0()
        {
            var original = new[] { 10 }.ToV2Enumerable();
            var foo = new FooExtension<int>(original).AsV2Enumerable();

            var appended = foo.Append(5);
            CollectionAssert.AreEqual(new[] { 5, 10 }, appended.ToArray());

            var prependedAppended = appended.Prepend(15);
            CollectionAssert.AreEqual(new[] { 15, 5, 10 }, prependedAppended.ToArray());

            var appendedPrependedAppended = prependedAppended.Append(20);
            CollectionAssert.AreEqual(new[] { 20, 15, 5, 10 }, appendedPrependedAppended.ToArray());

            var appendedAppendedPrependedAppended = appendedPrependedAppended.Append(25);
            CollectionAssert.AreEqual(new[] { 25, 20, 15, 5, 10 }, appendedAppendedPrependedAppended.ToArray());
        }

        [TestMethod]
        public void Test()
        {
            var original = new[] { 10 }.ToV2Enumerable();
            var foo = new FooExtension<int>(original).AsV2Enumerable();
            var bar = new BarExtension<int>(foo).AsV2Enumerable();

            // append should still delegate to "FooExtension"
            var appended = bar.Append(5);
            CollectionAssert.AreEqual(new[] { 5, 10 }, appended.ToArray());

            // prepend should still delegate to "BarExtension" even though "FooExtension" is what returned to us from the last call
            var prependedAppended = appended.Prepend(15);
            CollectionAssert.AreEqual(new[] { 5, 10, 15 }, prependedAppended.ToArray());
            
            // and now even though "BarExtension" is what just returned to us, we should still be calling "FooExtension"
            var appendedPrependedAppended = prependedAppended.Append(20);
            CollectionAssert.AreEqual(new[] { 20, 5, 10, 15 }, appendedPrependedAppended.ToArray());

            // and to close the loop, even though we have called through each extension once, we should still be able to get to the correct extension
            var prependedAppendedPrependedAppended = prependedAppended.Prepend(25);
            CollectionAssert.AreEqual(new[] { 20, 5, 10, 15, 25 }, prependedAppendedPrependedAppended.ToArray());
        }

        private sealed class FooExtension<T> : IEnumerableMonad<T>, IAppendableMixin<T>
        {
            public FooExtension(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return source => new FooExtension<TSource>(source);
            }

            public IV2Enumerable<T> Append(T element)
            {
                return AppendIterator(element).ToV2Enumerable();
            }

            private IEnumerable<T> AppendIterator(T element)
            {
                yield return element;
                foreach (var sourceElement in this.Source)
                {
                    yield return sourceElement;
                }
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

        private sealed class BarExtension<T> : IEnumerableMonad<T>, IPrependableMixin<T>
        {
            public BarExtension(IV2Enumerable<T> source)
            {
                this.Source = source;
            }

            public IV2Enumerable<T> Source { get; }

            public Unit<TSource> Unit<TSource>()
            {
                return source => new BarExtension<TSource>(source);
            }

            public IV2Enumerable<T> Prepend(T element)
            {
                return this.PrependIterator(element).ToV2Enumerable();
            }

            private IEnumerable<T> PrependIterator(T element)
            {
                foreach (var sourceElement in this.Source)
                {
                    yield return sourceElement;
                }

                yield return element;
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
