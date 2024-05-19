namespace System.Linq.V2
{
    public interface IAverageInt32Enumerable : IV2Enumerable<int>
    {
        public double Average()
        {
            return this.AverageDefault();
        }
    }
}