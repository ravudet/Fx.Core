namespace System.Linq.V2
{
    public interface ISumNullableInt32Enumerable : IV2Enumerable<int?>
    {
        public int? Sum()
        {
            return this.SumDefault();
        }
    }
}