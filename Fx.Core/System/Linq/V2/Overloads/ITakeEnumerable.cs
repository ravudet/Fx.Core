namespace System.Linq.V2
{
    public interface ITakeEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Take(Range range)
        {
            return this.TakeDefault(range);
        }

        public IV2Enumerable<TSource> Take(int count)
        {
            return this.TakeDefault(count);
        }
    }
}