namespace System.Linq.V2
{
    public interface ISelectEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TResult> Select<TResult>(Func<TSource, int, TResult> selector)
        {
            return this.SelectDefault(selector);
        }

        public IV2Enumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
        {
            return this.SelectDefault(selector);
        }
    }
}