namespace System.Linq.V2
{
    public interface IChunkableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource[]> Chunk(int size)
        {
            return this.ChunkDefault(size);
        }
    }
}