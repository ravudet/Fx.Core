namespace System.Linq.V2
{
    public interface ISelectManyEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TResult> SelectMany<TResult>(Func<TSource, int, IV2Enumerable<TResult>> selector)
        {
            return this.SelectManyDefault(selector);
        }

        public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(
            Func<TSource, int, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return this.SelectManyDefault(collectionSelector, resultSelector);
        }

        public IV2Enumerable<TResult> SelectMany<TResult>(Func<TSource, IV2Enumerable<TResult>> selector)
        {
            return this.SelectManyDefault(selector);
        }

        public IV2Enumerable<TResult> SelectMany<TCollection, TResult>(
            Func<TSource, IV2Enumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return this.SelectManyDefault(collectionSelector, resultSelector);
        }
    }
}