namespace System.Threading.Tasks
{
#if !NET8_0_OR_GREATER
    [Flags]
    public enum ConfigureAwaitOptions
    {
        None = 0,
        ContinueOnCapturedContext = 1 << 0,
        //// TODO you are intentionally not adding the other options because there's no way to support them with the framework's available functionality
    }
#endif
}
