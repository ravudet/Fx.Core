namespace System.Threading.Tasks
{
#if !NET8_0_OR_GREATER
    [Flags]
    public enum ConfigureAwaitOptions
    {
        None = 0,
        ContinueOnCapturedContext = 1 << 0,
        SuppressThrowing = 1 << 1,
        ForceYielding = 1 << 2,
    }
#endif
}
