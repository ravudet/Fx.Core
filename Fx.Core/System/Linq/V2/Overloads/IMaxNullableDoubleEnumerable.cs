namespace System.Linq.V2
{
    public interface IMaxNullableDoubleEnumerable : IV2Enumerable<double?>
    {
        public double? Max()
        {
            return this.MaxDefault();
        }
    }
}