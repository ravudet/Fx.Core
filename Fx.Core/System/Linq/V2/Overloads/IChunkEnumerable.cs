namespace System.Linq.V2
{
    public interface IChunkEnumerable<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource[]> Chunk(int size)
        {
            return this.ChunkDefault(size);
        }
    }
}