namespace System.Linq.V2
{
    public interface IAverageableDoubleMixin : IV2Enumerable<double>
    {
        public double Average()
        {
            return this.AverageDefault();
        }
    }
}