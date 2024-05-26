namespace System.Linq.V2
{
    public interface ISumableNullableInt32Mixin : IV2Enumerable<int?>
    {
        public int? Sum()
        {
            return this.SumDefault();
        }
    }
}