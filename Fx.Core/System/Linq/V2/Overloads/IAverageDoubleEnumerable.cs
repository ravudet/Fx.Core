namespace System.Linq.V2
{
    public interface IAverageDoubleEnumerable : IV2Enumerable<double>
    {
        public double Average()
        {
            return this.AverageDefault();
        }
    }
}