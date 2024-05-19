namespace System.Linq.V2
{
    public interface ISumDoubleEnumerable : IV2Enumerable<double>
    {
        public double Sum()
        {
            return this.SumDefault();
        }
    }
}