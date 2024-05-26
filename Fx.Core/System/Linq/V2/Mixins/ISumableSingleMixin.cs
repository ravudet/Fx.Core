namespace System.Linq.V2
{
    public interface ISumableSingleMixin : IV2Enumerable<float>
    {
        public float Sum()
        {
            return this.SumDefault();
        }
    }
}