namespace System.Linq.V2
{
    public interface IMinableNullableSingleMixin : IV2Enumerable<float?>
    {
        public float? Min()
        {
            return this.MinDefault();
        }
    }
}