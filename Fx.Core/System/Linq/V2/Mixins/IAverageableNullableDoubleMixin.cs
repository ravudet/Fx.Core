namespace System.Linq.V2
{
    public interface IAverageableNullableDoubleMixin : IV2Enumerable<double?>
    {
        public double? Average()
        {
            return this.AverageDefault();
        }
    }
}