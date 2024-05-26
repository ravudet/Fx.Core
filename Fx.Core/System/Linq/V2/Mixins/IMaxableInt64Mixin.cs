namespace System.Linq.V2
{
    public interface IMaxableInt64Mixin : IV2Enumerable<long>
    {
        public long Max()
        {
            return this.MaxDefault();
        }
    }
}