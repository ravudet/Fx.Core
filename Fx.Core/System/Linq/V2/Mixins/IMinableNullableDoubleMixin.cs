namespace System.Linq.V2
{
    public interface IMinableNullableDoubleMixin : IV2Enumerable<double?>
    {
        public double? Min()
        {
            return this.MinDefault();
        }
    }
}