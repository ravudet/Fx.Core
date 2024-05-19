namespace System.Linq.V2
{
    public interface ISumNullableDoubleEnumerable : IV2Enumerable<double?>
    {
        public double? Sum()
        {
            return this.SumDefault();
        }
    }
}