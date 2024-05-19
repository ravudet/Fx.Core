namespace System.Linq.V2
{
    public interface IDefaultIfEmptyEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource?> DefaultIfEmpty()
        {
            return this.DefaultIfEmptyDefault();
        }

        public IV2Enumerable<TSource> DefaultIfEmpty(TSource defaultValue)
        {
            return this.DefaultIfEmptyDefault(defaultValue);
        }
    }
}