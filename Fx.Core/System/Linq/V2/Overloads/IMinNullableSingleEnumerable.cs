namespace System.Linq.V2
{
    public interface IMinNullableSingleEnumerable : IV2Enumerable<float?>
    {
        public float? Min()
        {
            return this.MinDefault();
        }
    }
}