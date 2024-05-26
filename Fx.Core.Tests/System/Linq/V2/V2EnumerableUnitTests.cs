namespace System.Linq.V2
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;
    using System.Collections.Generic;

    [TestClass]
    public class V2EnumerableUnitTests
    {
        private const string uniqueTestValue1 = "2962B9CA-A5AF-466A-93EE-201EA84741CF";

        private const string uniqueTestValue2 = "A17B205B-C92A-4EEE-9FCF-37429FCE3754";

        private const string uniqueTestValue3 = "140E488A-01F3-495C-B0A7-BE732C52F13F";

        //// TODO should you remove iv2 : iv1? that way no one accidentally escapes back to v1?
        //// TODO you skpped the adapter methods (tov2enumerable, tov2lookup, etc.)

        [TestMethod]
        public void Aggregate()
        {
            var enumerable = new AggregateMock();
            var aggregated = enumerable.Aggregate((_, _) => uniqueTestValue1);
            Assert.AreEqual(uniqueTestValue1, aggregated);
        }

        [TestMethod]
        public void AggregateWithSeed()
        {
            var enumerable = new AggregateMock();
            var aggregated = enumerable.Aggregate(uniqueTestValue1 , (accumulate, _) => $"{accumulate}{uniqueTestValue2}");
            Assert.AreEqual($"{uniqueTestValue1}{uniqueTestValue2}", aggregated);
        }

        [TestMethod]
        public void AggregateWithSeedAndResultSelector()
        {
            var enumerable = new AggregateMock();
            var aggregated = enumerable.Aggregate(uniqueTestValue1, (accumulate, _) => $"{accumulate}{uniqueTestValue2}", accumulate => $"{accumulate}{uniqueTestValue3}");
            Assert.AreEqual($"{uniqueTestValue1}{uniqueTestValue2}{uniqueTestValue3}", aggregated);
        }

        private sealed class AggregateMock : IAggregatableMixin<string>
        {
            public AggregateMock()
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
    }
}