namespace System.Linq.V2
{
    public interface IMaxSingleEnumerable : IV2Enumerable<float>
    {
        public float Max()
        {
            return this.MaxDefault();
        }
    }
}