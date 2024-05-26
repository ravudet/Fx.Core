namespace System.Linq.V2
{
    public interface IPrependableMixin<TSource> : IV2Enumerable<TSource>
    {
        public IV2Enumerable<TSource> Prepend(TSource element)
        {
            return this.PrependDefault(element);
        }
    }
}