namespace System.Linq.V2
{
    public interface IMinableDoubleMixin : IV2Enumerable<double>
    {
        public double Min()
        {
            return this.MinDefault();
        }
    }
}