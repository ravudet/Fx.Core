namespace System.Linq.V2
{
    public interface IAverageSingleEnumerable : IV2Enumerable<float>
    {
        public float Average()
        {
            return this.AverageDefault();
        }
    }
}