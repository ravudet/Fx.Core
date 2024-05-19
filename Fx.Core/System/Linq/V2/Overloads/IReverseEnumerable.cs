namespace System.Linq.V2
{
    public interface IReverseEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Reverse()
        {
            return this.ReverseDefault();
        }
    }
}