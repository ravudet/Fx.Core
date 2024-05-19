namespace System.Linq.V2
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;

    public static partial class V2Enumerable
    {
        internal static TSource AggregateDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, TSource, TSource> func)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Aggregate(func);
            }

            return self.AsEnumerable().Aggregate(func);
        }

        internal static TAccumulate AggregateDefault<TSource, TAccumulate>(this IV2Enumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Aggregate(seed, func);
            }

            return self.AsEnumerable().Aggregate(seed, func);
        }

        internal static TResult AggregateDefault<TSource, TAccumulate, TResult>(
            this IV2Enumerable<TSource> self,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Aggregate(seed, func, resultSelector);
            }

            return self.AsEnumerable().Aggregate(seed, func, resultSelector);
        }

        internal static bool AllDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.All(predicate);
            }

            return self.AsEnumerable().All(predicate);
        }

        internal static bool AnyDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Any();
            }

            return self.AsEnumerable().Any();
        }

        internal static bool AnyDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Any(predicate);
            }

            return self.AsEnumerable().Any(predicate);
        }

        internal static IV2Enumerable<TSource> AppendDefault<TSource>(this IV2Enumerable<TSource> self, TSource element)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Append(element));
            }

            return self.AsEnumerable().Append(element).ToV2Enumerable();
        }

        internal static double AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static double AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static double? AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static float AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static double? AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static float? AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static double AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static double? AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static decimal AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static decimal? AverageDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average(selector);
            }

            return self.AsEnumerable().Average(selector);
        }

        internal static float? AverageDefault(this IV2Enumerable<float?> self)
        {
            if (self is IAggregatedOverloadEnumerable<float?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double? AverageDefault(this IV2Enumerable<long?> self)
        {
            if (self is IAggregatedOverloadEnumerable<long?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double? AverageDefault(this IV2Enumerable<int?> self)
        {
            if (self is IAggregatedOverloadEnumerable<int?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double? AverageDefault(this IV2Enumerable<double?> self)
        {
            if (self is IAggregatedOverloadEnumerable<double?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static decimal? AverageDefault(this IV2Enumerable<decimal?> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double AverageDefault(this IV2Enumerable<long> self)
        {
            if (self is IAggregatedOverloadEnumerable<long> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double AverageDefault(this IV2Enumerable<int> self)
        {
            if (self is IAggregatedOverloadEnumerable<int> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static double AverageDefault(this IV2Enumerable<double> self)
        {
            if (self is IAggregatedOverloadEnumerable<double> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static decimal AverageDefault(this IV2Enumerable<decimal> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        internal static float AverageDefault(this IV2Enumerable<float> self)
        {
            if (self is IAggregatedOverloadEnumerable<float> aggregatedOverload)
            {
                return aggregatedOverload.Source.Average();
            }

            return self.AsEnumerable().Average();
        }

        /*internal static IV2Enumerable<TResult> Cast<TResult>(this IV2Enumerable self)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        internal static IV2Enumerable<TSource[]> ChunkDefault<TSource>(this IV2Enumerable<TSource> self, int size)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Chunk(size));
            }

            return self.AsEnumerable().Chunk(size).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> ConcatDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Concat(second));
            }

            return first.AsEnumerable().Concat(second).ToV2Enumerable();
        }

        internal static bool ContainsDefault<TSource>(this IV2Enumerable<TSource> self, TSource value, IEqualityComparer<TSource>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Contains(value, comparer);
            }

            return self.AsEnumerable().Contains(value, comparer);
        }

        internal static bool ContainsDefault<TSource>(this IV2Enumerable<TSource> self, TSource value)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Contains(value);
            }

            return self.AsEnumerable().Contains(value);
        }

        internal static int CountDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Count();
            }

            return self.AsEnumerable().Count();
        }

        internal static int CountDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Count(predicate);
            }

            return self.AsEnumerable().Count(predicate);
        }

        internal static IV2Enumerable<TSource?> DefaultIfEmptyDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.DefaultIfEmpty());
            }

            return self.AsEnumerable().DefaultIfEmpty().ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> DefaultIfEmptyDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.DefaultIfEmpty(defaultValue));
            }

            return self.AsEnumerable().DefaultIfEmpty(defaultValue).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> DistinctDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Distinct());
            }

            return self.AsEnumerable().Distinct().ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> DistinctDefault<TSource>(this IV2Enumerable<TSource> self, IEqualityComparer<TSource>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Distinct(comparer));
            }

            return self.AsEnumerable().Distinct(comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> DistinctByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.DistinctBy(keySelector));
            }

            return self.AsEnumerable().DistinctBy(keySelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> DistinctByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.DistinctBy(keySelector, comparer));
            }

            return self.AsEnumerable().DistinctBy(keySelector, comparer).ToV2Enumerable();
        }

        internal static TSource ElementAtDefault<TSource>(this IV2Enumerable<TSource> self, Index index)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ElementAt(index);
            }

            return self.AsEnumerable().ElementAt(index);
        }

        internal static TSource ElementAtDefault<TSource>(this IV2Enumerable<TSource> self, int index)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ElementAt(index);
            }

            return self.AsEnumerable().ElementAt(index);
        }

        internal static TSource? ElementAtOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Index index)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ElementAtOrDefault(index);
            }

            return self.AsEnumerable().ElementAtOrDefault(index);
        }

        internal static TSource? ElementAtOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, int index)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ElementAtOrDefault(index);
            }

            return self.AsEnumerable().ElementAtOrDefault(index);
        }

        internal static IV2Enumerable<TResult> EmptyDefault<TResult>()
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        internal static IV2Enumerable<TSource> ExceptDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Except(second));
            }

            return first.AsEnumerable().Except(second).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> ExceptDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Except(second, comparer));
            }

            return first.AsEnumerable().Except(second, comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> ExceptByDefault<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.ExceptBy(second, keySelector));
            }

            return first.AsEnumerable().ExceptBy(second, keySelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> ExceptByDefault<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.ExceptBy(second, keySelector, comparer));
            }

            return first.AsEnumerable().ExceptBy(second, keySelector, comparer).ToV2Enumerable();
        }

        internal static TSource FirstDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.First();
            }

            return self.AsEnumerable().First();
        }

        internal static TSource FirstDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.First(predicate);
            }

            return self.AsEnumerable().First(predicate);
        }

        internal static TSource? FirstOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.FirstOrDefault();
            }

            return self.AsEnumerable().FirstOrDefault();
        }

        internal static TSource FirstOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.FirstOrDefault(defaultValue);
            }

            return self.AsEnumerable().FirstOrDefault(defaultValue);
        }

        internal static TSource? FirstOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.FirstOrDefault(predicate);
            }

            return self.AsEnumerable().FirstOrDefault(predicate);
        }

        internal static TSource FirstOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.FirstOrDefault(predicate, defaultValue);
            }

            return self.AsEnumerable().FirstOrDefault(predicate, defaultValue);
        }

        internal static IV2Enumerable<TResult> GroupByDefault<TSource, TKey, TElement, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, elementSelector, resultSelector, comparer));
            }

            return self.AsEnumerable().GroupBy(keySelector, elementSelector, (key, enumerable) => resultSelector(key, enumerable.ToV2Enumerable()), comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> GroupByDefault<TSource, TKey, TElement, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, elementSelector, resultSelector));
            }

            return self.AsEnumerable().GroupBy(keySelector, elementSelector, (key, enumerable) => resultSelector(key, enumerable.ToV2Enumerable())).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> GroupByDefault<TSource, TKey, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, resultSelector, comparer));
            }

            return self.AsEnumerable().GroupBy(keySelector, (key, enumerable) => resultSelector(key, enumerable.ToV2Enumerable()), comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> GroupByDefault<TSource, TKey, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, resultSelector));
            }

            return self.AsEnumerable().GroupBy(keySelector, (key, enumerable) => resultSelector(key, enumerable.ToV2Enumerable())).ToV2Enumerable();
        }

        internal static IV2Enumerable<IV2Grouping<TKey, TSource>> GroupByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector));
            }

            return self.AsEnumerable().GroupBy(keySelector).Select(ToV2Grouping).ToV2Enumerable();
        }

        internal static IV2Enumerable<IV2Grouping<TKey, TElement>> GroupByDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, elementSelector));
            }

            return self.AsEnumerable().GroupBy(keySelector, elementSelector).Select(ToV2Grouping).ToV2Enumerable();
        }

        internal static IV2Enumerable<IV2Grouping<TKey, TSource>> GroupByDefault<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, comparer));
            }

            return self.AsEnumerable().GroupBy(keySelector, comparer).Select(ToV2Grouping).ToV2Enumerable();
        }

        internal static IV2Enumerable<IV2Grouping<TKey, TElement>> GroupByDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupBy(keySelector, elementSelector, comparer));
            }

            return self.AsEnumerable().GroupBy(keySelector, elementSelector, comparer).Select(ToV2Grouping).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> GroupJoinDefault<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer is IAggregatedOverloadEnumerable<TOuter> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
            }

            return outer.AsEnumerable().GroupJoin(inner, outerKeySelector, innerKeySelector, (element, sequence) => resultSelector(element, sequence.ToV2Enumerable()), comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> GroupJoinDefault<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector)
        {
            if (outer is IAggregatedOverloadEnumerable<TOuter> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
            }

            return outer.AsEnumerable().GroupJoin(inner, outerKeySelector, innerKeySelector, (element, sequence) => resultSelector(element, sequence.ToV2Enumerable())).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> IntersectDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Intersect(second, comparer));
            }

            return first.AsEnumerable().Intersect(second, comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> IntersectDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Intersect(second));
            }

            return first.AsEnumerable().Intersect(second).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> IntersectByDefault<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.IntersectBy(second, keySelector));
            }

            return first.AsEnumerable().IntersectBy(second, keySelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> IntersectByDefault<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.IntersectBy(second, keySelector, comparer));
            }

            return first.AsEnumerable().IntersectBy(second, keySelector, comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> JoinDefault<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer is IAggregatedOverloadEnumerable<TOuter> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
            }

            return outer.AsEnumerable().Join(inner, outerKeySelector, innerKeySelector, resultSelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> JoinDefault<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer is IAggregatedOverloadEnumerable<TOuter> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
            }

            return outer.AsEnumerable().Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer).ToV2Enumerable();
        }

        internal static TSource LastDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Last();
            }

            return self.AsEnumerable().Last();
        }

        internal static TSource LastDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Last(predicate);
            }

            return self.AsEnumerable().Last(predicate);
        }

        internal static TSource? LastOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LastOrDefault();
            }

            return self.AsEnumerable().LastOrDefault();
        }

        internal static TSource LastOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LastOrDefault(defaultValue);
            }

            return self.AsEnumerable().LastOrDefault(defaultValue);
        }

        internal static TSource? LastOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LastOrDefault(predicate);
            }

            return self.AsEnumerable().LastOrDefault(predicate);
        }

        internal static TSource LastOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LastOrDefault(predicate, defaultValue);
            }

            return self.AsEnumerable().LastOrDefault(predicate, defaultValue);
        }

        internal static long LongCountDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LongCount(predicate);
            }

            return self.AsEnumerable().LongCount(predicate);
        }

        internal static long LongCountDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.LongCount();
            }

            return self.AsEnumerable().LongCount();
        }

        internal static long MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static decimal MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static double MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static int MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static decimal? MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static TSource? MaxDefault<TSource>(this IV2Enumerable<TSource> self, IComparer<TSource>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(comparer);
            }

            return self.AsEnumerable().Max(comparer);
        }

        internal static int? MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static long? MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static float? MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static TResult? MaxDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static double? MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static TSource? MaxDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static float MaxDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max(selector);
            }

            return self.AsEnumerable().Max(selector);
        }

        internal static float MaxDefault(this IV2Enumerable<float> self)
        {
            if (self is IAggregatedOverloadEnumerable<float> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static float? MaxDefault(this IV2Enumerable<float?> self)
        {
            if (self is IAggregatedOverloadEnumerable<float?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static long? MaxDefault(this IV2Enumerable<long?> self)
        {
            if (self is IAggregatedOverloadEnumerable<long?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static int? MaxDefault(this IV2Enumerable<int?> self)
        {
            if (self is IAggregatedOverloadEnumerable<int?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static double? MaxDefault(this IV2Enumerable<double?> self)
        {
            if (self is IAggregatedOverloadEnumerable<double?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static decimal? MaxDefault(this IV2Enumerable<decimal?> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static long MaxDefault(this IV2Enumerable<long> self)
        {
            if (self is IAggregatedOverloadEnumerable<long> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static int MaxDefault(this IV2Enumerable<int> self)
        {
            if (self is IAggregatedOverloadEnumerable<int> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static double MaxDefault(this IV2Enumerable<double> self)
        {
            if (self is IAggregatedOverloadEnumerable<double> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static decimal MaxDefault(this IV2Enumerable<decimal> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal> aggregatedOverload)
            {
                return aggregatedOverload.Source.Max();
            }

            return self.AsEnumerable().Max();
        }

        internal static TSource? MaxByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.MaxBy(keySelector);
            }

            return self.AsEnumerable().MaxBy(keySelector);
        }

        internal static TSource? MaxByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.MaxBy(keySelector, comparer);
            }

            return self.AsEnumerable().MaxBy(keySelector, comparer);
        }

        internal static decimal MinDefault(this IV2Enumerable<decimal> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static TResult? MinDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static float MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static float? MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static int? MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static double? MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static decimal? MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static long MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static int MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static decimal MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static TSource? MinDefault<TSource>(this IV2Enumerable<TSource> self, IComparer<TSource>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(comparer);
            }

            return self.AsEnumerable().Min(comparer);
        }

        internal static TSource? MinDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static long? MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static double MinDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min(selector);
            }

            return self.AsEnumerable().Min(selector);
        }

        internal static float MinDefault(this IV2Enumerable<float> self)
        {
            if (self is IAggregatedOverloadEnumerable<float> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static float? MinDefault(this IV2Enumerable<float?> self)
        {
            if (self is IAggregatedOverloadEnumerable<float?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static long? MinDefault(this IV2Enumerable<long?> self)
        {
            if (self is IAggregatedOverloadEnumerable<long?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static int? MinDefault(this IV2Enumerable<int?> self)
        {
            if (self is IAggregatedOverloadEnumerable<int?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static double? MinDefault(this IV2Enumerable<double?> self)
        {
            if (self is IAggregatedOverloadEnumerable<double?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static decimal? MinDefault(this IV2Enumerable<decimal?> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static double MinDefault(this IV2Enumerable<double> self)
        {
            if (self is IAggregatedOverloadEnumerable<double> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static long MinDefault(this IV2Enumerable<long> self)
        {
            if (self is IAggregatedOverloadEnumerable<long> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static int MinDefault(this IV2Enumerable<int> self)
        {
            if (self is IAggregatedOverloadEnumerable<int> aggregatedOverload)
            {
                return aggregatedOverload.Source.Min();
            }

            return self.AsEnumerable().Min();
        }

        internal static TSource? MinByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.MinBy(keySelector, comparer);
            }

            return self.AsEnumerable().MinBy(keySelector, comparer);
        }

        internal static TSource? MinByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.MinBy(keySelector);
            }

            return self.AsEnumerable().MinBy(keySelector);
        }

        /*internal static IV2Enumerable<TResult> OfType<TResult>(this IV2Enumerable self)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        internal static IV2OrderedEnumerable<TSource> OrderByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var ordered = aggregatedOverload.Source.OrderBy(keySelector, comparer);
                var aggregated = aggregatedOverload.Create(ordered);
                return new V2OrderedEnumerableAggregatedEnumerable<TSource>(ordered, aggregated);
            }

            return self.AsEnumerable().OrderBy(keySelector, comparer).ToV2OrderedEnumerable();
        }

        private sealed class V2OrderedEnumerableAggregatedEnumerable<TElement> : IV2OrderedEnumerable<TElement>, IAggregatedOverloadEnumerable<TElement>
        {
            private readonly IV2OrderedEnumerable<TElement> ordered;

            private readonly IAggregatedOverloadEnumerable<TElement> aggregated;

            public V2OrderedEnumerableAggregatedEnumerable(IV2OrderedEnumerable<TElement> ordered, IAggregatedOverloadEnumerable<TElement> aggregated)
            {
                this.ordered = ordered;
                this.aggregated = aggregated;
            }

            public IV2Enumerable<TElement> Source => this.ordered;

            public IAggregatedOverloadEnumerable<TSource> Create<TSource>(IV2Enumerable<TSource> source)
            {
                return this.aggregated.Create(source);
            }

            public IV2OrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
            {
                return this.ordered.CreateOrderedEnumerable(keySelector, comparer, descending);
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                return this.aggregated.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        internal static IV2OrderedEnumerable<TSource> OrderByDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var ordered = aggregatedOverload.Source.OrderBy(keySelector);
                var aggregated = aggregatedOverload.Create(ordered);
                return new V2OrderedEnumerableAggregatedEnumerable<TSource>(ordered, aggregated);
            }

            return self.AsEnumerable().OrderBy(keySelector).ToV2OrderedEnumerable();
        }

        internal static IV2OrderedEnumerable<TSource> OrderByDescendingDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var ordered = aggregatedOverload.Source.OrderByDescending(keySelector);
                var aggregated = aggregatedOverload.Create(ordered);
                return new V2OrderedEnumerableAggregatedEnumerable<TSource>(ordered, aggregated);
            }

            return self.AsEnumerable().OrderByDescending(keySelector).ToV2OrderedEnumerable();
        }

        internal static IV2OrderedEnumerable<TSource> OrderByDescendingDefault<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var ordered = aggregatedOverload.Source.OrderByDescending(keySelector, comparer);
                var aggregated = aggregatedOverload.Create(ordered);
                return new V2OrderedEnumerableAggregatedEnumerable<TSource>(ordered, aggregated);
            }

            return self.AsEnumerable().OrderByDescending(keySelector, comparer).ToV2OrderedEnumerable();
        }

        internal static IV2Enumerable<TSource> PrependDefault<TSource>(this IV2Enumerable<TSource> self, TSource element)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Prepend(element));
            }

            return self.AsEnumerable().Prepend(element).ToV2Enumerable();
        }

        /*internal static IV2Enumerable<int> Range(int start, int count)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        /*internal static IV2Enumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        internal static IV2Enumerable<TSource> ReverseDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Reverse());
            }

            return self.AsEnumerable().Reverse().ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, int, TResult> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Select(selector));
            }

            return self.AsEnumerable().Select(selector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Select(selector));
            }

            return self.AsEnumerable().Select(selector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectManyDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, int, IV2Enumerable<TResult>> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SelectMany(selector));
            }

            return self.AsEnumerable().SelectMany(selector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectManyDefault<TSource, TCollection, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SelectMany(collectionSelector, resultSelector));
            }

            return self.AsEnumerable().SelectMany(collectionSelector, resultSelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectManyDefault<TSource, TCollection, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, int, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SelectMany(collectionSelector, resultSelector));
            }

            return self.AsEnumerable().SelectMany(collectionSelector, resultSelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> SelectManyDefault<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, IV2Enumerable<TResult>> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SelectMany(selector));
            }

            return self.AsEnumerable().SelectMany(selector).ToV2Enumerable();
        }

        internal static bool SequenceEqualDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SequenceEqual(second);
            }

            return first.AsEnumerable().SequenceEqual(second);
        }

        internal static bool SequenceEqualDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SequenceEqual(second, comparer);
            }

            return first.AsEnumerable().SequenceEqual(second, comparer);
        }

        internal static TSource SingleDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Single();
            }

            return self.AsEnumerable().Single();
        }

        internal static TSource SingleDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Single(predicate);
            }

            return self.AsEnumerable().Single(predicate);
        }

        internal static TSource SingleOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SingleOrDefault(predicate, defaultValue);
            }

            return self.AsEnumerable().SingleOrDefault(predicate, defaultValue);
        }

        internal static TSource SingleOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SingleOrDefault(defaultValue);
            }

            return self.AsEnumerable().SingleOrDefault(defaultValue);
        }

        internal static TSource? SingleOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SingleOrDefault();
            }

            return self.AsEnumerable().SingleOrDefault();
        }

        internal static TSource? SingleOrDefaultDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.SingleOrDefault(predicate);
            }

            return self.AsEnumerable().SingleOrDefault(predicate);
        }

        internal static IV2Enumerable<TSource> SkipDefault<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Skip(count));
            }

            return self.AsEnumerable().Skip(count).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> SkipLastDefault<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SkipLast(count));
            }

            return self.AsEnumerable().SkipLast(count).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> SkipWhileDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SkipWhile(predicate));
            }

            return self.AsEnumerable().SkipWhile(predicate).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> SkipWhileDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.SkipWhile(predicate));
            }

            return self.AsEnumerable().SkipWhile(predicate).ToV2Enumerable();
        }

        internal static int SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static long SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static decimal? SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static long? SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static int? SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static double SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static float? SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static float SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static double? SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static decimal SumDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum(selector);
            }

            return self.AsEnumerable().Sum(selector);
        }

        internal static long? SumDefault(this IV2Enumerable<long?> self)
        {
            if (self is IAggregatedOverloadEnumerable<long?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static float? SumDefault(this IV2Enumerable<float?> self)
        {
            if (self is IAggregatedOverloadEnumerable<float?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static int? SumDefault(this IV2Enumerable<int?> self)
        {
            if (self is IAggregatedOverloadEnumerable<int?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static double? SumDefault(this IV2Enumerable<double?> self)
        {
            if (self is IAggregatedOverloadEnumerable<double?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static decimal? SumDefault(this IV2Enumerable<decimal?> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal?> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static long SumDefault(this IV2Enumerable<long> self)
        {
            if (self is IAggregatedOverloadEnumerable<long> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static int SumDefault(this IV2Enumerable<int> self)
        {
            if (self is IAggregatedOverloadEnumerable<int> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static double SumDefault(this IV2Enumerable<double> self)
        {
            if (self is IAggregatedOverloadEnumerable<double> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static decimal SumDefault(this IV2Enumerable<decimal> self)
        {
            if (self is IAggregatedOverloadEnumerable<decimal> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static float SumDefault(this IV2Enumerable<float> self)
        {
            if (self is IAggregatedOverloadEnumerable<float> aggregatedOverload)
            {
                return aggregatedOverload.Source.Sum();
            }

            return self.AsEnumerable().Sum();
        }

        internal static IV2Enumerable<TSource> TakeDefault<TSource>(this IV2Enumerable<TSource> self, Range range)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Take(range));
            }

            return self.AsEnumerable().Take(range).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> TakeDefault<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Take(count));
            }

            return self.AsEnumerable().Take(count).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> TakeLastDefault<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.TakeLast(count));
            }

            return self.AsEnumerable().TakeLast(count).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> TakeWhileDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.TakeWhile(predicate));
            }

            return self.AsEnumerable().TakeWhile(predicate).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> TakeWhileDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.TakeWhile(predicate));
            }

            return self.AsEnumerable().TakeWhile(predicate).ToV2Enumerable();
        }

        /*internal static IV2OrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        internal static IV2OrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        internal static IV2OrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        internal static IV2OrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IV2OrderedEnumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        internal static TSource[] ToArrayDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToArray();
            }

            return self.AsEnumerable().ToArray();
        }

        internal static Dictionary<TKey, TSource> ToDictionaryDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
            where TKey : notnull
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToDictionary(keySelector);
            }

            return self.AsEnumerable().ToDictionary(keySelector);
        }

        internal static Dictionary<TKey, TSource> ToDictionaryDefault<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToDictionary(keySelector, comparer);
            }

            return self.AsEnumerable().ToDictionary(keySelector, comparer);
        }

        internal static Dictionary<TKey, TElement> ToDictionaryDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
            where TKey : notnull
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToDictionary(keySelector, elementSelector);
            }

            return self.AsEnumerable().ToDictionary(keySelector, elementSelector);
        }

        internal static Dictionary<TKey, TElement> ToDictionaryDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToDictionary(keySelector, elementSelector, comparer);
            }

            return self.AsEnumerable().ToDictionary(keySelector, elementSelector, comparer);
        }

        internal static HashSet<TSource> ToHashSetDefault<TSource>(this IV2Enumerable<TSource> self, IEqualityComparer<TSource>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToHashSet(comparer);
            }

            return self.AsEnumerable().ToHashSet(comparer);
        }

        internal static HashSet<TSource> ToHashSetDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToHashSet();
            }

            return self.AsEnumerable().ToHashSet();
        }

        internal static List<TSource> ToListDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.ToList();
            }

            return self.AsEnumerable().ToList();
        }

        internal static IV2Lookup<TKey, TElement> ToLookupDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var lookup = aggregatedOverload.Source.ToLookup(keySelector, elementSelector, comparer);
                var aggregated = aggregatedOverload.Create(lookup);
                return new V2LookupAggregatedOverload<TKey, TElement>(lookup, aggregated);
            }

            return self.AsEnumerable().ToLookup(keySelector, elementSelector, comparer).ToV2Lookup();
        }

        private sealed class V2LookupAggregatedOverload<TKey, TElement> : IV2Lookup<TKey, TElement>, IAggregatedOverloadEnumerable<IV2Grouping<TKey, TElement>>
        {
            private readonly IV2Lookup<TKey, TElement> lookup;

            private readonly IAggregatedOverloadEnumerable<IV2Grouping<TKey, TElement>> aggregated;

            public V2LookupAggregatedOverload(IV2Lookup<TKey, TElement> lookup, IAggregatedOverloadEnumerable<IV2Grouping<TKey, TElement>> aggregated)
            {
                this.lookup = lookup;
                this.aggregated = aggregated;
            }

            public IV2Enumerable<TElement> this[TKey key] => this.lookup[key];

            public IV2Enumerable<IV2Grouping<TKey, TElement>> Source => this.lookup;

            public int Count => this.lookup.Count;

            public bool Contains(TKey key)
            {
                return this.lookup.Contains(key);
            }

            public IAggregatedOverloadEnumerable<TSource> Create<TSource>(IV2Enumerable<TSource> source)
            {
                return this.aggregated.Create(source);
            }

            public IEnumerator<IV2Grouping<TKey, TElement>> GetEnumerator()
            {
                return this.aggregated.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        internal static IV2Lookup<TKey, TElement> ToLookupDefault<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var lookup = aggregatedOverload.Source.ToLookup(keySelector, elementSelector);
                var aggregated = aggregatedOverload.Create(lookup);
                return new V2LookupAggregatedOverload<TKey, TElement>(lookup, aggregated);
            }

            return self.AsEnumerable().ToLookup(keySelector, elementSelector).ToV2Lookup();
        }

        internal static IV2Lookup<TKey, TSource> ToLookupDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var lookup = aggregatedOverload.Source.ToLookup(keySelector);
                var aggregated = aggregatedOverload.Create(lookup);
                return new V2LookupAggregatedOverload<TKey, TSource>(lookup, aggregated);
            }

            return self.AsEnumerable().ToLookup(keySelector).ToV2Lookup();
        }

        internal static IV2Lookup<TKey, TSource> ToLookupDefault<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                var lookup = aggregatedOverload.Source.ToLookup(keySelector, comparer);
                var aggregated = aggregatedOverload.Create(lookup);
                return new V2LookupAggregatedOverload<TKey, TSource>(lookup, aggregated);
            }

            return self.AsEnumerable().ToLookup(keySelector, comparer).ToV2Lookup();
        }

        internal static bool TryGetNonEnumeratedCountDefault<TSource>(this IV2Enumerable<TSource> self, out int count)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Source.TryGetNonEnumeratedCount(out count);
            }

            return self.AsEnumerable().TryGetNonEnumeratedCount(out count);
        }

        internal static IV2Enumerable<TSource> UnionDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Union(second));
            }

            return first.AsEnumerable().Union(second).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> UnionDefault<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Union(second, comparer));
            }

            return first.AsEnumerable().Union(second, comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> UnionByDefault<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, Func<TSource, TKey> keySelector)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.UnionBy(second, keySelector));
            }

            return first.AsEnumerable().UnionBy(second, keySelector).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> UnionByDefault<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.UnionBy(second, keySelector, comparer));
            }

            return first.AsEnumerable().UnionBy(second, keySelector, comparer).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> WhereDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Where(predicate));
            }

            return self.AsEnumerable().Where(predicate).ToV2Enumerable();
        }

        internal static IV2Enumerable<TSource> WhereDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is IAggregatedOverloadEnumerable<TSource> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Where(predicate));
            }

            return self.AsEnumerable().Where(predicate).ToV2Enumerable();
        }

        internal static IV2Enumerable<(TFirst First, TSecond Second, TThird Third)> ZipDefault<TFirst, TSecond, TThird>(
            this IV2Enumerable<TFirst> first,
            IV2Enumerable<TSecond> second,
            IV2Enumerable<TThird> third)
        {
            if (first is IAggregatedOverloadEnumerable<TFirst> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Zip(second, third));
            }

            return first.AsEnumerable().Zip(second, third).ToV2Enumerable();
        }

        internal static IV2Enumerable<(TFirst First, TSecond Second)> ZipDefault<TFirst, TSecond>(this IV2Enumerable<TFirst> first, IV2Enumerable<TSecond> second)
        {
            if (first is IAggregatedOverloadEnumerable<TFirst> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Zip(second));
            }

            return first.AsEnumerable().Zip(second).ToV2Enumerable();
        }

        internal static IV2Enumerable<TResult> ZipDefault<TFirst, TSecond, TResult>(
            this IV2Enumerable<TFirst> first,
            IV2Enumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first is IAggregatedOverloadEnumerable<TFirst> aggregatedOverload)
            {
                return aggregatedOverload.Create(aggregatedOverload.Source.Zip(second, resultSelector));
            }

            return first.AsEnumerable().Zip(second, resultSelector).ToV2Enumerable();
        }
    }
}
