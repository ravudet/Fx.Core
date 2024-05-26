namespace System.Linq.V2
{
    public interface IMinableNullableInt64Mixin : IV2Enumerable<long?>
    {
        public long? Min()
        {
            return this.MinDefault();
        }
    }
}