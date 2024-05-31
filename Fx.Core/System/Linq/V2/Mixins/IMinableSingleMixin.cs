namespace System.Linq.V2
{
    public interface IMinableSingleMixin : IV2Enumerable<float>
    {
        public float Min()
        {
            return this.MinDefault();
        }
    }
}