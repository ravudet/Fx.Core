namespace System.Linq.V2
{
    public interface IAverageableNullableInt64Mixin : IV2Enumerable<long?>
    {
        public double? Average()
        {
            return this.AverageDefault();
        }
    }
}