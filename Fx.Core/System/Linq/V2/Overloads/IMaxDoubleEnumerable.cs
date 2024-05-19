namespace System.Linq.V2
{
    public interface IMaxDoubleEnumerable : IV2Enumerable<double>
    {
        public double Max()
        {
            return this.MaxDefault();
        }
    }
}