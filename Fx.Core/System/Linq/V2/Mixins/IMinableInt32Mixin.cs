namespace System.Linq.V2
{
    public interface IMinableInt32Mixin : IV2Enumerable<int>
    {
        public int Min()
        {
            return this.MinDefault();
        }
    }
}