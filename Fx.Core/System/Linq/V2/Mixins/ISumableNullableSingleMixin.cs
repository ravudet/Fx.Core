namespace System.Linq.V2
{
    public interface ISumableNullableSingleMixin : IV2Enumerable<float?>
    {
        public float? Sum()
        {
            return this.SumDefault();
        }
    }
}