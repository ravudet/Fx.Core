namespace System.Linq.V2
{
    public interface IAverageableSingleMixin : IV2Enumerable<float>
    {
        public float Average()
        {
            return this.AverageDefault();
        }
    }
}