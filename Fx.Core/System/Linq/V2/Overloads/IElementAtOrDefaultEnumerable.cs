namespace System.Linq.V2
{
    public interface IElementAtOrDefaultEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource? ElementAtOrDefault(Index index)
        {
            return this.ElementAtOrDefaultDefault(index);
        }

        public TSource? ElementAtOrDefault(int index)
        {
            return this.ElementAtOrDefaultDefault(index);
        }
    }
}