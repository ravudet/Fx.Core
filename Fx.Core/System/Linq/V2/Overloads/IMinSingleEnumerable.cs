namespace System.Linq.V2
{
    public interface IMinSingleEnumerable : IV2Enumerable<float> //// TODO remind yourself that you could just use default interface implementations for all of linq; however, this doesn't establish the pattern for others to add their own extension methods and introduce their own interfaces; further, there can be conflicts between the interface and existing concrete collection implementations (or such things could be introduced in the future); those conflicts can cause compiler errors when upgrading linq versions in certain cases
    {
        public float Min()
        {
            return this.MinDefault();
        }
    }
}