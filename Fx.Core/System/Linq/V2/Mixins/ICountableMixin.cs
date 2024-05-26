namespace System.Linq.V2
{
    public interface ICountableMixin<TSource> : IV2Enumerable<TSource>
    {
        public int Count()
        {
            return this.CountDefault();
        }

        public int Count(Func<TSource, bool> predicate)
        {
            return this.CountDefault(predicate);
        }
    }
}