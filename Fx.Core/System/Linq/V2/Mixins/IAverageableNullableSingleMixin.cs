namespace System.Linq.V2
{
    public interface IAverageableNullableSingleMixin : IV2Enumerable<float?>
    {
        public float? Average()
        {
            return this.AverageDefault();
        }
    }
}