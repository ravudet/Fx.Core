namespace System.Linq.V2
{
    public interface IMaxableSingleMixin : IV2Enumerable<float>
    {
        public float Max()
        {
            return this.MaxDefault();
        }
    }
}