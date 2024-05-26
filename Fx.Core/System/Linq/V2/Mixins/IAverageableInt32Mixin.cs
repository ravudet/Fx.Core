namespace System.Linq.V2
{
    public interface IAverageableInt32Mixin : IV2Enumerable<int>
    {
        public double Average()
        {
            return this.AverageDefault();
        }
    }
}