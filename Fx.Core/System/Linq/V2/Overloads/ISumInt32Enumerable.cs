namespace System.Linq.V2
{
    public interface ISumInt32Enumerable : IV2Enumerable<int>
    {
        public int Sum()
        {
            return this.SumDefault();
        }
    }
}