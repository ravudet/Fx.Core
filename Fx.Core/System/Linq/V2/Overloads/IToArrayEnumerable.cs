namespace System.Linq.V2
{
    public interface IToArrayEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource[] ToArray()
        {
            return this.ToArrayDefault();
        }
    }
}