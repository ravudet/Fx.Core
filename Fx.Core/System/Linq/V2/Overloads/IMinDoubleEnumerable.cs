namespace System.Linq.V2
{
    public interface IMinDoubleEnumerable : IV2Enumerable<double>
    {
        public double Min()
        {
            return this.MinDefault();
        }
    }
}