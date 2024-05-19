namespace System.Linq.V2
{
    public interface ISumNullableSingleEnumerable : IV2Enumerable<float?>
    {
        public float? Sum()
        {
            return this.SumDefault();
        }
    }
}