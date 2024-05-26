namespace System.Linq.V2
{
    public interface IConcatableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Concat(IV2Enumerable<TSource> second)
        {
            return this.ConcatDefault(second);
        }
    }
}