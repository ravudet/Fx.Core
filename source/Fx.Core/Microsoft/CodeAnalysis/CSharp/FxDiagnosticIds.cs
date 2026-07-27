namespace Microsoft.CodeAnalysis.CSharp
{
    public static class FxDiagnosticIds
    {
        private const string IdPrefix = "FX"; // it is suggested not to use two-character prefixes [here](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/choosing-diagnostic-ids#considerations), but that's our name, so i am just going with it

        public const string BaseLowercaseTypeNameAnalyzerDiagnosticId = $"{FxDiagnosticIds.IdPrefix}0001";
    }
}
