namespace System.Linq.V2
{
    public interface IElementAtEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public TSource ElementAt(Index index)
        {
            return this.ElementAtDefault(index);
        }

        public TSource ElementAt(int index)
        {
            return this.ElementAtDefault(index);
        }
    }
}