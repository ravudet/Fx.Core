namespace System.Linq.V2
{
    public interface IConcatEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Concat(IV2Enumerable<TSource> second)
        {
            return this.ConcatDefault(second);
        }
    }
}