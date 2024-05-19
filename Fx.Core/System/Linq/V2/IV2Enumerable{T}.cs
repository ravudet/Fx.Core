namespace System.Linq.V2
{
    using System.Collections.Generic;

    //// TODO get christof's feedback on the interfaces
    //// TODO get christof's feedback on the extension methods
    ////
    //// TODO fix any spacing issues in the interface files in the overloads folder
    //// TODO check if you should make anything public that's internal or private
    public interface IV2Enumerable<out T> : IEnumerable<T>, IV2Enumerable
    {
    }
}