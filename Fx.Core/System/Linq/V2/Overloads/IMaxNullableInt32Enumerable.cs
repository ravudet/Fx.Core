namespace System.Linq.V2
{
    public interface IMaxNullableInt32Enumerable : IV2Enumerable<int?>
    {
        public int? Max()
        {
            return this.MaxDefault();
        }
    }
}