namespace System.Linq.V2
{
    public interface IMaxInt32Enumerable : IV2Enumerable<int>
    {
        public int Max()
        {
            return this.MaxDefault();
        }
    }
}