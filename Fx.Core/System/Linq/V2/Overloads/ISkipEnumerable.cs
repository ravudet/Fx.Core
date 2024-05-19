namespace System.Linq.V2
{
    public interface ISkipEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Skip(int count)
        {
            return this.SkipDefault(count);
        }
    }
}