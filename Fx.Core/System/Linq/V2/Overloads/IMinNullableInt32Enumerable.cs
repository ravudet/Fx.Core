namespace System.Linq.V2
{
    public interface IMinNullableInt32Enumerable : IV2Enumerable<int?>
    {
        public int? Min()
        {
            return this.MinDefault();
        }
    }
}