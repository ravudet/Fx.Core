namespace System.Linq.V2
{
    public interface IAverageNullableDoubleEnumerable : IV2Enumerable<double?>
    {
        public double? Average()
        {
            return this.AverageDefault();
        }
    }
}