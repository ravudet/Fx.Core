namespace System.Linq.V2
{
    public interface ITryGetNonEnumeratedCountableMixin<TSource> : IV2Enumerable<TSource>
    {
        public bool TryGetNonEnumeratedCount(out int count)
        {
            return this.TryGetNonEnumeratedCountDefault(out count);
        }
    }
}