namespace System.Linq.V2
{
    public interface IMinableNullableInt32Mixin : IV2Enumerable<int?>
    {
        public int? Min()
        {
            return this.MinDefault();
        }
    }
}