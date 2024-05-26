namespace System.Linq.V2
{
    public interface IFirstableMixin<TSource> : IV2Enumerable<TSource>
    {
        public TSource First()
        {
            return this.FirstDefault();
        }

        public TSource First(Func<TSource, bool> predicate)
        {
            return this.FirstDefault(predicate);
        }
    }
}