namespace System.Linq.V2
{
    public interface IAverageInt64Enumerable : IV2Enumerable<long>
    {
        public double Average()
        {
            return this.AverageDefault();
        }
    }
}