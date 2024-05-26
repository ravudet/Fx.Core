namespace System.Linq.V2
{
    public interface IMaxableDoubleMixin : IV2Enumerable<double>
    {
        public double Max()
        {
            return this.MaxDefault();
        }
    }
}