namespace System.Linq.V2
{
    public interface ISumableDoubleMixin : IV2Enumerable<double>
    {
        public double Sum()
        {
            return this.SumDefault();
        }
    }
}