namespace System.Linq.V2
{
    public interface IMaxNullableSingleEnumerable : IV2Enumerable<float?>
    {
        public float? Max()
        {
            return this.MaxDefault();
        }
    }
}