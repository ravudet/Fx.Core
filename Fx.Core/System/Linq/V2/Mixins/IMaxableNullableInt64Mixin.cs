namespace System.Linq.V2
{
    public interface IMaxableNullableInt64Mixin : IV2Enumerable<long?>
    {
        public long? Max()
        {
            return this.MaxDefault();
        }
    }
}