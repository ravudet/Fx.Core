namespace System.Linq.V2
{
    public interface IMaxableNullableSingleMixin : IV2Enumerable<float?>
    {
        public float? Max()
        {
            return this.MaxDefault();
        }
    }
}