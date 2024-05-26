namespace System.Linq.V2
{
    public interface IMaxableNullableDoubleMixin : IV2Enumerable<double?>
    {
        public double? Max()
        {
            return this.MaxDefault();
        }
    }
}