﻿namespace System.Linq.V2
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
        public static IV2Enumerable<TElement> ToV2Enumerable<TElement>(this IEnumerable<TElement> self)
        {
            return new DefaultV2Enumerable<TElement>(self);
        }

        private sealed class DefaultV2Enumerable<TElement> : IV2Enumerable<TElement>
        {
            private readonly IEnumerable<TElement> self;

            public DefaultV2Enumerable(IEnumerable<TElement> self)
            {
                this.self = self;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                return this.self.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private static IV2Grouping<TKey, TElement> ToV2Grouping<TKey, TElement>(this IGrouping<TKey, TElement> grouping)
        {
            return new DefaultV2Grouping<TKey, TElement>(grouping);
        }

        private sealed class DefaultV2Grouping<TKey, TElement> : IV2Grouping<TKey, TElement>
        {
            private readonly IGrouping<TKey, TElement> grouping;

            public DefaultV2Grouping(IGrouping<TKey, TElement> grouping)
            {
                this.grouping = grouping;
            }

            public TKey Key
            {
                get
                {
                    return this.grouping.Key;
                }
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                return this.grouping.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private static IV2OrderedEnumerable<TElement> ToV2OrderedEnumerable<TElement>(this IOrderedEnumerable<TElement> orderedEnumerable)
        {
            return new DefaultV2OrderedEnumerable<TElement>(orderedEnumerable);
        }

        private sealed class DefaultV2OrderedEnumerable<TElement> : IV2OrderedEnumerable<TElement>
        {
            private readonly IOrderedEnumerable<TElement> orderedEnumerable;

            public DefaultV2OrderedEnumerable(IOrderedEnumerable<TElement> orderedEnumerable)
            {
                this.orderedEnumerable = orderedEnumerable;
            }

            public IV2OrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
            {
                return this.orderedEnumerable.CreateOrderedEnumerable(keySelector, comparer, descending).ToV2OrderedEnumerable();
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                return this.orderedEnumerable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private static IV2Lookup<TKey, TElement> ToV2Lookup<TKey, TElement>(this ILookup<TKey, TElement> lookup)
        {
            return new DefaultV2Lookup<TKey, TElement>(lookup);
        }

        private sealed class DefaultV2Lookup<TKey, TElement> : IV2Lookup<TKey, TElement>
        {
            private readonly ILookup<TKey, TElement> lookup;

            public DefaultV2Lookup(ILookup<TKey, TElement> lookup)
            {
                this.lookup = lookup;
            }

            public IV2Enumerable<TElement> this[TKey key]
            {
                get
                {
                    return this.lookup[key].ToV2Enumerable();
                }
            }

            public int Count
            {
                get
                {
                    return this.lookup.Count;
                }
            }

            public bool Contains(TKey key)
            {
                return this.lookup.Contains(key);
            }

            public IEnumerator<IV2Grouping<TKey, TElement>> GetEnumerator()
            {
                return this.lookup.Select(ToV2Grouping).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }

    public static partial class V2Enumerable
    {
        public static TSource Aggregate<TSource>(this IV2Enumerable<TSource> self, Func<TSource, TSource, TSource> func)
        {
            if (self is IAggregatableMixin<TSource> aggregate)
            {
                return aggregate.Aggregate(func);
            }

            return self.AggregateDefault(func);
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IV2Enumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (self is IAggregatableMixin<TSource> aggregate)
            {
                return aggregate.Aggregate(seed, func);
            }

            return self.AggregateDefault(seed, func);
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this IV2Enumerable<TSource> self,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            if (self is IAggregatableMixin<TSource> aggregate)
            {
                return aggregate.Aggregate(seed, func, resultSelector);
            }

            return self.AggregateDefault(seed, func, resultSelector);
        }

        public static bool All<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAllableMixin<TSource> all)
            {
                return all.All(predicate);
            }

            return self.AllDefault(predicate);
        }

        public static bool Any<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IAnyableMixin<TSource> any)
            {
                return any.Any();
            }

            return self.AnyDefault();
        }

        public static bool Any<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IAnyableMixin<TSource> any)
            {
                return any.Any(predicate);
            }

            return self.AnyDefault(predicate);
        }

        public static IV2Enumerable<TSource> Append<TSource>(this IV2Enumerable<TSource> self, TSource element)
        {
            if (self is IAppendableMixin<TSource> append)
            {
                return append.Append(element);
            }

            return self.AppendDefault(element);
        }

        public static IV2Enumerable<TSource> AsV2Enumerable<TSource>(this IV2Enumerable<TSource> self)
        {
            //// TODO this extension is named differently than v1 linq; this is *probably* a good thing
            return self;
        }

        public static double Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static double Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static double? Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static float Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static double? Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static float? Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static double Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static double? Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static decimal Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static decimal? Average<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IAverageableMixin<TSource> average)
            {
                return average.Average(selector);
            }

            return self.AverageDefault(selector);
        }

        public static float? Average(this IV2Enumerable<float?> self)
        {
            if (self is IAverageableNullableSingleMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double? Average(this IV2Enumerable<long?> self)
        {
            if (self is IAverageableNullableInt64Mixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double? Average(this IV2Enumerable<int?> self)
        {
            if (self is IAverageableNullableInt32Mixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double? Average(this IV2Enumerable<double?> self)
        {
            if (self is IAverageableNullableDoubleMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static decimal? Average(this IV2Enumerable<decimal?> self)
        {
            if (self is IAverageableNullableDecimalMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double Average(this IV2Enumerable<long> self)
        {
            if (self is IAverageableInt64Mixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double Average(this IV2Enumerable<int> self)
        {
            if (self is IAverageableInt32Mixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static double Average(this IV2Enumerable<double> self)
        {
            if (self is IAverageableDoubleMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static decimal Average(this IV2Enumerable<decimal> self)
        {
            if (self is IAverageableDecimalMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        public static float Average(this IV2Enumerable<float> self)
        {
            if (self is IAverageableSingleMixin average)
            {
                return average.Average();
            }

            return self.AverageDefault();
        }

        /*public static IV2Enumerable<TResult> Cast<TResult>(this IV2Enumerable self)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        public static IV2Enumerable<TSource[]> Chunk<TSource>(this IV2Enumerable<TSource> self, int size)
        {
            if (self is IChunkableMixin<TSource> chunk)
            {
                return chunk.Chunk(size);
            }

            return self.ChunkDefault(size);
        }

        public static IV2Enumerable<TSource> Concat<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IConcatableMixin<TSource> concat)
            {
                return concat.Concat(second);
            }

            return first.ConcatDefault(second);
        }

        public static bool Contains<TSource>(this IV2Enumerable<TSource> self, TSource value, IEqualityComparer<TSource>? comparer)
        {
            if (self is IContainsableMixin<TSource> contains)
            {
                return contains.Contains(value, comparer);
            }

            return self.ContainsDefault(value, comparer);
        }

        public static bool Contains<TSource>(this IV2Enumerable<TSource> self, TSource value)
        {
            if (self is IContainsableMixin<TSource> contains)
            {
                return contains.Contains(value);
            }

            return self.ContainsDefault(value);
        }

        public static int Count<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ICountableMixin<TSource> count)
            {
                return count.Count();
            }

            return self.CountDefault();
        }

        public static int Count<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ICountableMixin<TSource> count)
            {
                return count.Count(predicate);
            }

            return self.CountDefault(predicate);
        }

        public static IV2Enumerable<TSource?> DefaultIfEmpty<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IDefaultIfEmptyableMixin<TSource> defaultIfEmpty)
            {
                return defaultIfEmpty.DefaultIfEmpty();
            }

            return self.DefaultIfEmptyDefault();
        }

        public static IV2Enumerable<TSource> DefaultIfEmpty<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IDefaultIfEmptyableMixin<TSource> defaultIfEmpty)
            {
                return defaultIfEmpty.DefaultIfEmpty(defaultValue);
            }

            return self.DefaultIfEmptyDefault(defaultValue);
        }

        public static IV2Enumerable<TSource> Distinct<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IDistinctableMixin<TSource> distinct)
            {
                return distinct.Distinct();
            }

            return self.DistinctDefault();
        }

        public static IV2Enumerable<TSource> Distinct<TSource>(this IV2Enumerable<TSource> self, IEqualityComparer<TSource>? comparer)
        {
            if (self is IDistinctableMixin<TSource> distinct)
            {
                return distinct.Distinct(comparer);
            }

            return self.DistinctDefault(comparer);
        }

        public static IV2Enumerable<TSource> DistinctBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IDistinctByableMixin<TSource> distinctBy)
            {
                return distinctBy.DistinctBy(keySelector);
            }

            return self.DistinctByDefault(keySelector);
        }

        public static IV2Enumerable<TSource> DistinctBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (self is IDistinctByableMixin<TSource> distinctBy)
            {
                return distinctBy.DistinctBy(keySelector, comparer);
            }

            return self.DistinctByDefault(keySelector, comparer);
        }

        public static TSource ElementAt<TSource>(this IV2Enumerable<TSource> self, Index index)
        {
            if (self is IElementAtableMixin<TSource> elementAt)
            {
                return elementAt.ElementAt(index);
            }

            return self.ElementAtDefault(index);
        }

        public static TSource ElementAt<TSource>(this IV2Enumerable<TSource> self, int index)
        {
            if (self is IElementAtableMixin<TSource> elementAt)
            {
                return elementAt.ElementAt(index);
            }

            return self.ElementAtDefault(index);
        }

        public static TSource? ElementAtOrDefault<TSource>(this IV2Enumerable<TSource> self, Index index)
        {
            if (self is IElementAtOrDefaultableMixin<TSource> elementAtOrDefault)
            {
                return elementAtOrDefault.ElementAtOrDefault(index);
            }

            return self.ElementAtOrDefaultDefault(index);
        }

        public static TSource? ElementAtOrDefault<TSource>(this IV2Enumerable<TSource> self, int index)
        {
            if (self is IElementAtOrDefaultableMixin<TSource> elementAtOrDefault)
            {
                return elementAtOrDefault.ElementAtOrDefault(index);
            }

            return self.ElementAtOrDefaultDefault(index);
        }

        public static IV2Enumerable<TResult> Empty<TResult>()
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        public static IV2Enumerable<TSource> Except<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IExceptableMixin<TSource> except)
            {
                return except.Except(second);
            }

            return first.ExceptDefault(second);
        }

        public static IV2Enumerable<TSource> Except<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IExceptableMixin<TSource> except)
            {
                return except.Except(second, comparer);
            }

            return first.ExceptDefault(second, comparer);
        }

        public static IV2Enumerable<TSource> ExceptBy<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            if (first is IExceptByableMixin<TSource> exceptBy)
            {
                return exceptBy.ExceptBy(second, keySelector);
            }

            return first.ExceptByDefault(second, keySelector);
        }

        public static IV2Enumerable<TSource> ExceptBy<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IExceptByableMixin<TSource> exceptBy)
            {
                return exceptBy.ExceptBy(second, keySelector, comparer);
            }

            return first.ExceptByDefault(second, keySelector);
        }

        public static TSource First<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IFirstableMixin<TSource> first)
            {
                return first.First();
            }

            return self.FirstDefault();
        }

        public static TSource First<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IFirstableMixin<TSource> first)
            {
                return first.First(predicate);
            }

            return self.FirstDefault(predicate);
        }

        public static TSource? FirstOrDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IFirstOrDefaultableMixin<TSource> firstOrDefault)
            {
                return firstOrDefault.FirstOrDefault();
            }

            return self.FirstOrDefaultDefault();
        }

        public static TSource FirstOrDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is IFirstOrDefaultableMixin<TSource> firstOrDefault)
            {
                return firstOrDefault.FirstOrDefault(defaultValue);
            }

            return self.FirstOrDefaultDefault(defaultValue);
        }

        public static TSource? FirstOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IFirstOrDefaultableMixin<TSource> firstOrDefault)
            {
                return firstOrDefault.FirstOrDefault(predicate);
            }

            return self.FirstOrDefaultDefault(predicate);
        }

        public static TSource FirstOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is IFirstOrDefaultableMixin<TSource> firstOrDefault)
            {
                return firstOrDefault.FirstOrDefault(predicate, defaultValue);
            }

            return self.FirstOrDefaultDefault(predicate, defaultValue);
        }

        public static IV2Enumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, elementSelector, resultSelector, comparer);
            }

            return self.GroupByDefault(keySelector, elementSelector, resultSelector, comparer);
        }

        public static IV2Enumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IV2Enumerable<TElement>, TResult> resultSelector)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, elementSelector, resultSelector);
            }

            return self.GroupByDefault(keySelector, elementSelector, resultSelector);
        }

        public static IV2Enumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, resultSelector, comparer);
            }

            return self.GroupByDefault(keySelector, resultSelector, comparer);
        }

        public static IV2Enumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TKey, IV2Enumerable<TSource>, TResult> resultSelector)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, resultSelector);
            }

            return self.GroupByDefault(keySelector, resultSelector);
        }

        public static IV2Enumerable<IV2Grouping<TKey, TSource>> GroupBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector);
            }

            return self.GroupByDefault(keySelector);
        }

        public static IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, elementSelector);
            }

            return self.GroupByDefault(keySelector, elementSelector);
        }

        public static IV2Enumerable<IV2Grouping<TKey, TSource>> GroupBy<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, comparer);
            }

            return self.GroupByDefault(keySelector, comparer);
        }

        public static IV2Enumerable<IV2Grouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IGroupByableMixin<TSource> groupBy)
            {
                return groupBy.GroupBy(keySelector, elementSelector, comparer);
            }

            return self.GroupByDefault(keySelector, elementSelector, comparer);
        }

        public static IV2Enumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer is IGroupJoinableMixin<TOuter> groupJoin)
            {
                return groupJoin.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            return outer.GroupJoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IV2Enumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IV2Enumerable<TInner>, TResult> resultSelector)
        {
            if (outer is IGroupJoinableMixin<TOuter> groupJoin)
            {
                return groupJoin.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
            }

            return outer.GroupJoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IV2Enumerable<TSource> Intersect<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IIntersectableMixin<TSource> intersect)
            {
                return intersect.Intersect(second, comparer);
            }

            return first.IntersectDefault(second, comparer);
        }

        public static IV2Enumerable<TSource> Intersect<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IIntersectableMixin<TSource> intersect)
            {
                return intersect.Intersect(second);
            }

            return first.IntersectDefault(second);
        }

        public static IV2Enumerable<TSource> IntersectBy<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TKey> second, Func<TSource, TKey> keySelector)
        {
            if (first is IIntersectByableMixin<TSource> intersectBy)
            {
                return intersectBy.IntersectBy(second, keySelector);
            }

            return first.IntersectByDefault(second, keySelector);
        }

        public static IV2Enumerable<TSource> IntersectBy<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IIntersectByableMixin<TSource> intersectBy)
            {
                return intersectBy.IntersectBy(second, keySelector, comparer);
            }

            return first.IntersectByDefault(second, keySelector, comparer);
        }

        public static IV2Enumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer is IJoinableMixin<TOuter> join)
            {
                return join.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
            }

            return outer.JoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IV2Enumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IV2Enumerable<TOuter> outer,
            IV2Enumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer is IJoinableMixin<TOuter> join)
            {
                return join.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            return outer.JoinDefault(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static TSource Last<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ILastableMixin<TSource> last)
            {
                return last.Last();
            }

            return self.LastDefault();
        }

        public static TSource Last<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ILastableMixin<TSource> last)
            {
                return last.Last(predicate);
            }

            return self.LastDefault(predicate);
        }

        public static TSource? LastOrDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ILastOrDefaultableMixin<TSource> lastOrDefault)
            {
                return lastOrDefault.LastOrDefault();
            }

            return self.LastOrDefaultDefault();
        }

        public static TSource LastOrDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is ILastOrDefaultableMixin<TSource> lastOrDefault)
            {
                return lastOrDefault.LastOrDefault(defaultValue);
            }

            return self.LastOrDefaultDefault(defaultValue);
        }

        public static TSource? LastOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ILastOrDefaultableMixin<TSource> lastOrDefault)
            {
                return lastOrDefault.LastOrDefault(predicate);
            }

            return self.LastOrDefaultDefault(predicate);
        }

        public static TSource LastOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is ILastOrDefaultableMixin<TSource> lastOrDefault)
            {
                return lastOrDefault.LastOrDefault(predicate, defaultValue);
            }

            return self.LastOrDefaultDefault(predicate, defaultValue);
        }

        public static long LongCount<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ILongCountableMixin<TSource> longCount)
            {
                return longCount.LongCount(predicate);
            }

            return self.LongCountDefault(predicate);
        }

        public static long LongCount<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ILongCountableMixin<TSource> longCount)
            {
                return longCount.LongCount();
            }

            return self.LongCountDefault();
        }

        public static long Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static decimal Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static double Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static int Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static decimal? Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static TSource? Max<TSource>(this IV2Enumerable<TSource> self, IComparer<TSource>? comparer)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(comparer);
            }

            return self.MaxDefault(comparer);
        }

        public static int? Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static long? Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static float? Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static TResult? Max<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static double? Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static TSource? Max<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static float Max<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IMaxableMixin<TSource> max)
            {
                return max.Max(selector);
            }

            return self.MaxDefault(selector);
        }

        public static float Max(this IV2Enumerable<float> self)
        {
            if (self is IMaxableSingleMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static float? Max(this IV2Enumerable<float?> self)
        {
            if (self is IMaxableNullableSingleMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static long? Max(this IV2Enumerable<long?> self)
        {
            if (self is IMaxableNullableInt64Mixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static int? Max(this IV2Enumerable<int?> self)
        {
            if (self is IMaxableNullableInt32Mixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static double? Max(this IV2Enumerable<double?> self)
        {
            if (self is IMaxableNullableDoubleMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static decimal? Max(this IV2Enumerable<decimal?> self)
        {
            if (self is IMaxableNullableDecimalMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static long Max(this IV2Enumerable<long> self)
        {
            if (self is IMaxableInt64Mixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static int Max(this IV2Enumerable<int> self)
        {
            if (self is IMaxableInt32Mixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static double Max(this IV2Enumerable<double> self)
        {
            if (self is IMaxableDoubleMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static decimal Max(this IV2Enumerable<decimal> self)
        {
            if (self is IMaxableDecimalMixin max)
            {
                return max.Max();
            }

            return self.MaxDefault();
        }

        public static TSource? MaxBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IMaxByableMixin<TSource> maxBy)
            {
                return maxBy.MaxBy(keySelector);
            }

            return self.MaxByDefault(keySelector);
        }

        public static TSource? MaxBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IMaxByableMixin<TSource> maxBy)
            {
                return maxBy.MaxBy(keySelector, comparer);
            }

            return self.MaxByDefault(keySelector, comparer);
        }

        public static decimal Min(this IV2Enumerable<decimal> self)
        {
            if (self is IMinableDecimalMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static TResult? Min<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static float Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static float? Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static int? Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static double? Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static decimal? Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static long Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static int Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static decimal Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static TSource? Min<TSource>(this IV2Enumerable<TSource> self, IComparer<TSource>? comparer)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(comparer);
            }

            return self.MinDefault(comparer);
        }

        public static TSource? Min<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static long? Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static double Min<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is IMinableMixin<TSource> min)
            {
                return min.Min(selector);
            }

            return self.MinDefault(selector);
        }

        public static float Min(this IV2Enumerable<float> self)
        {
            if (self is IMinableSingleMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static float? Min(this IV2Enumerable<float?> self)
        {
            if (self is IMinableNullableSingleMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static long? Min(this IV2Enumerable<long?> self)
        {
            if (self is IMinableNullableInt64Mixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static int? Min(this IV2Enumerable<int?> self)
        {
            if (self is IMinableNullableInt32Mixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static double? Min(this IV2Enumerable<double?> self)
        {
            if (self is IMinableNullableDoubleMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static decimal? Min(this IV2Enumerable<decimal?> self)
        {
            if (self is IMinableNullableDecimalMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static double Min(this IV2Enumerable<double> self)
        {
            if (self is IMinableDoubleMixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static long Min(this IV2Enumerable<long> self)
        {
            if (self is IMinableInt64Mixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static int Min(this IV2Enumerable<int> self)
        {
            if (self is IMinableInt32Mixin min)
            {
                return min.Min();
            }

            return self.MinDefault();
        }

        public static TSource? MinBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IMinByableMixin<TSource> minBy)
            {
                return minBy.MinBy(keySelector, comparer);
            }

            return self.MinByDefault(keySelector, comparer);
        }

        public static TSource? MinBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IMinByableMixin<TSource> minBy)
            {
                return minBy.MinBy(keySelector);
            }

            return self.MinByDefault(keySelector);
        }

        /*public static IV2Enumerable<TResult> OfType<TResult>(this IV2Enumerable self)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        public static IV2OrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (self is IOrderByableMixin<TSource> orderBy)
            {
                return orderBy.OrderBy(keySelector, comparer);
            }

            return self.OrderByDefault(keySelector, comparer);
        }

        public static IV2OrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IOrderByableMixin<TSource> orderBy)
            {
                return orderBy.OrderBy(keySelector);
            }

            return self.OrderByDefault(keySelector);
        }

        public static IV2OrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IOrderByDescendingableMixin<TSource> orderByDescending)
            {
                return orderByDescending.OrderByDescending(keySelector);
            }

            return self.OrderByDescendingDefault(keySelector);
        }

        public static IV2OrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (self is IOrderByDescendingableMixin<TSource> orderByDescending)
            {
                return orderByDescending.OrderByDescending(keySelector, comparer);
            }

            return self.OrderByDescendingDefault(keySelector);
        }

        public static IV2Enumerable<TSource> Prepend<TSource>(this IV2Enumerable<TSource> self, TSource element)
        {
            if (self is IPrependableMixin<TSource> prepend)
            {
                return prepend.Prepend(element);
            }

            return self.PrependDefault(element);
        }

        /*public static IV2Enumerable<int> Range(int start, int count)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        /*public static IV2Enumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        public static IV2Enumerable<TSource> Reverse<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IReverseableMixin<TSource> reverse)
            {
                return reverse.Reverse();
            }

            return self.ReverseDefault();
        }

        public static IV2Enumerable<TResult> Select<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, int, TResult> selector)
        {
            if (self is ISelectableMixin<TSource> select)
            {
                return select.Select(selector);            
            }

            return self.SelectDefault(selector);
        }

        public static IV2Enumerable<TResult> Select<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, TResult> selector)
        {
            if (self is ISelectableMixin<TSource> select)
            {
                return select.Select(selector);
            }

            return self.SelectDefault(selector);
        }

        public static IV2Enumerable<TResult> SelectMany<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, int, IV2Enumerable<TResult>> selector)
        {
            if (self is ISelectManyableMixin<TSource> selectMany)
            {
                return selectMany.SelectMany(selector);
            }

            return self.SelectManyDefault(selector);
        }

        public static IV2Enumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (self is ISelectManyableMixin<TSource> selectMany)
            {
                return selectMany.SelectMany(collectionSelector, resultSelector);
            }

            return self.SelectManyDefault(collectionSelector, resultSelector);
        }

        public static IV2Enumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IV2Enumerable<TSource> self,
            Func<TSource, int, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (self is ISelectManyableMixin<TSource> selectMany)
            {
                return selectMany.SelectMany(collectionSelector, resultSelector);
            }

            return self.SelectManyDefault(collectionSelector, resultSelector);
        }

        public static IV2Enumerable<TResult> SelectMany<TSource, TResult>(this IV2Enumerable<TSource> self, Func<TSource, IV2Enumerable<TResult>> selector)
        {
            if (self is ISelectManyableMixin<TSource> selectMany)
            {
                return selectMany.SelectMany(selector);
            }

            return self.SelectManyDefault(selector);
        }

        public static bool SequenceEqual<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is ISequenceEqualableMixin<TSource> sequenceEqual)
            {
                return sequenceEqual.SequenceEqual(second);
            }

            return first.SequenceEqualDefault(second);
        }

        public static bool SequenceEqual<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is ISequenceEqualableMixin<TSource> sequenceEqual)
            {
                return sequenceEqual.SequenceEqual(second, comparer);
            }

            return first.SequenceEqualDefault(second, comparer);
        }

        public static TSource Single<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ISingleableMixin<TSource> single)
            {
                return single.Single();
            }

            return self.SingleDefault();
        }

        public static TSource Single<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ISingleableMixin<TSource> single)
            {
                return single.Single(predicate);
            }

            return self.SingleDefault(predicate);
        }

        public static TSource SingleOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self is ISingleOrDefaultableMixin<TSource> singleOrDefault)
            {
                return singleOrDefault.SingleOrDefault(predicate, defaultValue);
            }

            return self.SingleOrDefaultDefault(predicate, defaultValue);
        }

        public static TSource SingleOrDefault<TSource>(this IV2Enumerable<TSource> self, TSource defaultValue)
        {
            if (self is ISingleOrDefaultableMixin<TSource> singleOrDefault)
            {
                return singleOrDefault.SingleOrDefault(defaultValue);
            }

            return self.SingleOrDefaultDefault(defaultValue);
        }

        public static TSource? SingleOrDefault<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is ISingleOrDefaultableMixin<TSource> singleOrDefault)
            {
                return singleOrDefault.SingleOrDefault();
            }

            return self.SingleOrDefaultDefault();
        }

        public static TSource? SingleOrDefault<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ISingleOrDefaultableMixin<TSource> singleOrDefault)
            {
                return singleOrDefault.SingleOrDefault(predicate);
            }

            return self.SingleOrDefaultDefault(predicate);
        }

        public static IV2Enumerable<TSource> Skip<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is ISkipableMixin<TSource> skip)
            {
                return skip.Skip(count);
            }

            return self.SkipDefault(count);
        }

        public static IV2Enumerable<TSource> SkipLast<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is ISkipLastableMixin<TSource> skipLast)
            {
                return skipLast.SkipLast(count);
            }

            return self.SkipLastDefault(count);
        }

        public static IV2Enumerable<TSource> SkipWhile<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ISkipWhileableMixin<TSource> skipWhile)
            {
                return skipWhile.SkipWhile(predicate);
            }

            return self.SkipWhileDefault(predicate);
        }

        public static IV2Enumerable<TSource> SkipWhile<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is ISkipWhileableMixin<TSource> skipWhile)
            {
                return skipWhile.SkipWhile(predicate);
            }

            return self.SkipWhileDefault(predicate);
        }

        public static int Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static long Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static decimal? Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal?> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static long? Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, long?> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static int? Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int?> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static double Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static float? Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float?> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static float Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, float> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static double? Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, double?> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static decimal Sum<TSource>(this IV2Enumerable<TSource> self, Func<TSource, decimal> selector)
        {
            if (self is ISumableMixin<TSource> sum)
            {
                return sum.Sum(selector);
            }

            return self.SumDefault(selector);
        }

        public static long? Sum(this IV2Enumerable<long?> self)
        {
            if (self is ISumableNullableInt64Mixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static float? Sum(this IV2Enumerable<float?> self)
        {
            if (self is ISumableNullableSingleMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static int? Sum(this IV2Enumerable<int?> self)
        {
            if (self is ISumableNullableInt32Mixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static double? Sum(this IV2Enumerable<double?> self)
        {
            if (self is ISumableNullableDoubleMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static decimal? Sum(this IV2Enumerable<decimal?> self)
        {
            if (self is ISumableNullableDecimalMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static long Sum(this IV2Enumerable<long> self)
        {
            if (self is ISumableInt64Mixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static int Sum(this IV2Enumerable<int> self)
        {
            if (self is ISumableInt32Mixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static double Sum(this IV2Enumerable<double> self)
        {
            if (self is ISumableDoubleMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static decimal Sum(this IV2Enumerable<decimal> self)
        {
            if (self is ISumableDecimalMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static float Sum(this IV2Enumerable<float> self)
        {
            if (self is ISumableSingleMixin sum)
            {
                return sum.Sum();
            }

            return self.SumDefault();
        }

        public static IV2Enumerable<TSource> Take<TSource>(this IV2Enumerable<TSource> self, Range range)
        {
            if (self is ITakeableMixin<TSource> take)
            {
                return take.Take(range);
            }

            return self.TakeDefault(range);
        }

        public static IV2Enumerable<TSource> Take<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is ITakeableMixin<TSource> take)
            {
                return take.Take(count);
            }

            return self.TakeDefault(count);
        }

        public static IV2Enumerable<TSource> TakeLast<TSource>(this IV2Enumerable<TSource> self, int count)
        {
            if (self is ITakeLastableMixin<TSource> takeLast)
            {
                return takeLast.TakeLast(count);
            }

            return self.TakeLastDefault(count);
        }

        public static IV2Enumerable<TSource> TakeWhile<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is ITakeWhileableMixin<TSource> takeWhile)
            {
                return takeWhile.TakeWhile(predicate);
            }

            return self.TakeWhileDefault(predicate);
        }

        public static IV2Enumerable<TSource> TakeWhile<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is ITakeWhileableMixin<TSource> takeWhile)
            {
                return takeWhile.TakeWhile(predicate);
            }

            return self.TakeWhileDefault(predicate);
        }

        /*public static IV2OrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        public static IV2OrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        public static IV2OrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IV2OrderedEnumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            //// TODO
            throw new System.NotImplementedException();
        }

        public static IV2OrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IV2OrderedEnumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            //// TODO
            throw new System.NotImplementedException();
        }*/

        public static TSource[] ToArray<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IToArrayableMixin<TSource> toArray)
            {
                return toArray.ToArray();
            }

            return self.ToArrayDefault();
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
            where TKey : notnull
        {
            if (self is IToDictionaryableMixin<TSource> toDictionary)
            {
                return toDictionary.ToDictionary(keySelector);
            }

            return self.ToDictionaryDefault(keySelector);
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            if (self is IToDictionaryableMixin<TSource> toDictionary)
            {
                return toDictionary.ToDictionary(keySelector, comparer);
            }

            return self.ToDictionaryDefault(keySelector, comparer);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
            where TKey : notnull
        {
            if (self is IToDictionaryableMixin<TSource> toDictionary)
            {
                return toDictionary.ToDictionary(keySelector, elementSelector);
            }

            return self.ToDictionaryDefault(keySelector, elementSelector);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            if (self is IToDictionaryableMixin<TSource> toDictionary)
            {
                return toDictionary.ToDictionary(keySelector, elementSelector, comparer);
            }

            return self.ToDictionaryDefault(keySelector, elementSelector, comparer);
        }

        public static HashSet<TSource> ToHashSet<TSource>(this IV2Enumerable<TSource> self, IEqualityComparer<TSource>? comparer)
        {
            if (self is IToHashSetableMixin<TSource> toHashSet)
            {
                return toHashSet.ToHashSet(comparer);
            }

            return self.ToHashSetDefault(comparer);
        }

        public static HashSet<TSource> ToHashSet<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IToHashSetableMixin<TSource> toHashSet)
            {
                return toHashSet.ToHashSet();
            }

            return self.ToHashSetDefault();
        }

        public static List<TSource> ToList<TSource>(this IV2Enumerable<TSource> self)
        {
            if (self is IToListableMixin<TSource> toList)
            {
                return toList.ToList();
            }

            return self.ToListDefault();
        }

        public static IV2Lookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (self is IToLookupableMixin<TSource> toLookup)
            {
                return toLookup.ToLookup(keySelector, elementSelector, comparer);
            }

            return self.ToLookupDefault(keySelector, elementSelector, comparer);
        }

        public static IV2Lookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IV2Enumerable<TSource> self,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            if (self is IToLookupableMixin<TSource> toLookup)
            {
                return toLookup.ToLookup(keySelector, elementSelector);
            }

            return self.ToLookupDefault(keySelector, elementSelector);
        }

        public static IV2Lookup<TKey, TSource> ToLookup<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector)
        {
            if (self is IToLookupableMixin<TSource> toLookup)
            {
                return toLookup.ToLookup(keySelector);
            }

            return self.ToLookupDefault(keySelector);
        }

        public static IV2Lookup<TKey, TSource> ToLookup<TSource, TKey>(this IV2Enumerable<TSource> self, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (self is IToLookupableMixin<TSource> toLookup)
            {
                return toLookup.ToLookup(keySelector, comparer);
            }

            return self.ToLookupDefault(keySelector, comparer);
        }

        public static bool TryGetNonEnumeratedCount<TSource>(this IV2Enumerable<TSource> self, out int count)
        {
            if (self is ITryGetNonEnumeratedCountableMixin<TSource> tryGetNonEnumeratedCount)
            {
                return tryGetNonEnumeratedCount.TryGetNonEnumeratedCount(out count);
            }

            return self.TryGetNonEnumeratedCountDefault(out count);
        }

        public static IV2Enumerable<TSource> Union<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second)
        {
            if (first is IUnionableMixin<TSource> union)
            {
                return union.Union(second);
            }

            return first.UnionDefault(second);
        }

        public static IV2Enumerable<TSource> Union<TSource>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first is IUnionableMixin<TSource> union)
            {
                return union.Union(second, comparer);
            }

            return first.UnionDefault(second, comparer);
        }

        public static IV2Enumerable<TSource> UnionBy<TSource, TKey>(this IV2Enumerable<TSource> first, IV2Enumerable<TSource> second, Func<TSource, TKey> keySelector)
        {
            if (first is IUnionByableMixin<TSource> unionBy)
            {
                return unionBy.UnionBy(second, keySelector);
            }

            return first.UnionByDefault(second, keySelector);
        }

        public static IV2Enumerable<TSource> UnionBy<TSource, TKey>(
            this IV2Enumerable<TSource> first,
            IV2Enumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is IUnionByableMixin<TSource> unionBy)
            {
                return unionBy.UnionBy(second, keySelector, comparer);
            }

            return first.UnionByDefault(second, keySelector, comparer);
        }

        public static IV2Enumerable<TSource> Where<TSource>(this IV2Enumerable<TSource> self, Func<TSource, bool> predicate)
        {
            if (self is IWhereableMixin<TSource> where)
            {
                return where.Where(predicate);
            }

            return self.WhereDefault(predicate);
        }

        public static IV2Enumerable<TSource> Where<TSource>(this IV2Enumerable<TSource> self, Func<TSource, int, bool> predicate)
        {
            if (self is IWhereableMixin<TSource> where)
            {
                return where.Where(predicate);
            }

            return self.WhereDefault(predicate);
        }

        public static IV2Enumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(
            this IV2Enumerable<TFirst> first,
            IV2Enumerable<TSecond> second,
            IV2Enumerable<TThird> third)
        {
            if (first is IZipableMixin<TFirst> zip)
            {
                return zip.Zip(second, third);
            }

            return first.ZipDefault(second, third);
        }

        public static IV2Enumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IV2Enumerable<TFirst> first, IV2Enumerable<TSecond> second)
        {
            if (first is IZipableMixin<TFirst> zip)
            {
                return zip.Zip(second);
            }

            return first.ZipDefault(second);
        }

        public static IV2Enumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IV2Enumerable<TFirst> first,
            IV2Enumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first is IZipableMixin<TFirst> zip)
            {
                return zip.Zip(second, resultSelector);
            }

            return first.ZipDefault(second, resultSelector);
        }
    }
}
