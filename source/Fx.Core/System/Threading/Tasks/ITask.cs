namespace System.Threading.Tasks
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// TODO you are following the structure that the .net `task` has where task.configureawait returns a new type in a separate hierarchy; you *could* have `itask` not be configurable, though, and have a "configurable" task that implements `itask` and `configureawait` returns `itask`; TODO though maybe if `itask` implements `icontinuable`, it actually makes sense to preserve the current structure; i think really you need to make a decision of if you are creating interface parallels for the existing .net types, or if you are "fixing" the .net types
    /// 
    /// TODO this should implement `icontinuable`
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITask<out T> : IConfigurableAwaitable<IAwaiter<T>, T, IConfiguredTask<T>, IAwaiter<T>>
#if NET6_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Exception? Exception { get; }

        bool IsCanceled { get; }
    }
}
