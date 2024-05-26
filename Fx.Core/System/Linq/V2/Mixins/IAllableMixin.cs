namespace System.Linq.V2
{
    public interface IAllableMixin<TSource> : IV2Enumerable<TSource>
    {
        public bool All(Func<TSource, bool> predicate)
        {
            return this.AllDefault(predicate);
        }
    }
}