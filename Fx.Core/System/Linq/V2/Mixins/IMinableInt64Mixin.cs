namespace System.Linq.V2
{
    public interface IMinableInt64Mixin : IV2Enumerable<long>
    {
        public long Min()
        {
            return this.MinDefault();
        }
    }
}