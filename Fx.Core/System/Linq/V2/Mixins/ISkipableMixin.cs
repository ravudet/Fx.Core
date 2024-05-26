namespace System.Linq.V2
{
    public interface ISkipableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Skip(int count)
        {
            return this.SkipDefault(count);
        }
    }
}