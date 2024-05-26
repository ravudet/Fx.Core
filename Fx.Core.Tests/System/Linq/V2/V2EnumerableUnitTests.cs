namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;

    [TestClass]
    public class V2EnumerableUnitTests
    {
        //// TODO should you remove iv2 : iv1? that way no one accidentally escapes back to v1?
        //// TODO you skpped the adapter methods (tov2enumerable, tov2lookup, etc.)

        [TestMethod]
        public void Aggregate()
        {
            Func<string, string, string> func = (_, _) => "asdf";
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.Aggregate(func);
            Assert.AreEqual("asdf", aggregated);

            // make sure v1 has different behavior
            Assert.ThrowsException<InvalidOperationException>(() => enumerable.AsEnumerable().Aggregate(func));
        }

        [TestMethod]
        public void AggregateWithSeed()
        {
            var seed = "asdf";
            Func<string, string, string> func = (_, _) => "qwer";
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.Aggregate(seed, func);
            Assert.AreEqual("qwer", aggregated);

            // make sure v1 has different behavior
            Assert.AreEqual("asdf", enumerable.AsEnumerable().Aggregate(seed, func));
        }

        [TestMethod]
        public void AggregateWithSeedAndResultSelector()
        {
            var seed = "asdf";
            Func<string, string, string> func = (_, _) => "qwer";
            Func<string, string> resultSelector = _ => _;
            var enumerable = new AggregatableMock();

            var aggregated = enumerable.Aggregate(seed, func, resultSelector);
            Assert.AreEqual("qwer", aggregated);

            // make sure v1 has different behavior
            Assert.AreEqual("asdf", enumerable.AsEnumerable().Aggregate(seed, func, resultSelector));
        }

        private sealed class AggregatableMock : IAggregatableMixin<string>
        {
            public AggregatableMock()
            {
            }
            public string Aggregate(Func<string, string, string> func)
            {
                return func(string.Empty, string.Empty);
            }

            public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Func<TAccumulate, string, TAccumulate> func)
            {
                return func(seed, string.Empty);
            }

            public TResult Aggregate<TAccumulate, TResult>(
                TAccumulate seed,
                Func<TAccumulate, string, TAccumulate> func,
                Func<TAccumulate, TResult> resultSelector)
            {
                return resultSelector(func(seed, string.Empty));
            }

            public IEnumerator<string> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [TestMethod]
        public void All()
        {
        }

        private sealed class AllableMock : IAllableMixin<string>
        {
            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}