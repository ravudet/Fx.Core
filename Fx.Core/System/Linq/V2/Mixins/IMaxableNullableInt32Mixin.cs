namespace System.Linq.V2
{
    public interface IMaxableNullableInt32Mixin : IV2Enumerable<int?>
    {
        public int? Max()
        {
            return this.MaxDefault();
        }
    }
}