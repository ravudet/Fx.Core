namespace System.Linq.V2
{
    public interface ISkipLastableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> SkipLast(int count)
        {
            return this.SkipLastDefault(count);
        }
    }
}