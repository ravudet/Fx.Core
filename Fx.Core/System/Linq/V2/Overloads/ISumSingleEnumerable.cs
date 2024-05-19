namespace System.Linq.V2
{
    public interface ISumSingleEnumerable : IV2Enumerable<float>
    {
        public float Sum()
        {
            return this.SumDefault();
        }
    }
}