namespace System.Linq.V2
{
    public interface ISumableNullableDoubleMixin : IV2Enumerable<double?>
    {
        public double? Sum()
        {
            return this.SumDefault();
        }
    }
}