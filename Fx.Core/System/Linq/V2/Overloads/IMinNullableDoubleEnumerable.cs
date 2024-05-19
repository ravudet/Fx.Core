namespace System.Linq.V2
{
    public interface IMinNullableDoubleEnumerable : IV2Enumerable<double?>
    {
        public double? Min()
        {
            return this.MinDefault();
        }
    }
}