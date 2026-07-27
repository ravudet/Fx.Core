namespace Microsoft.CodeAnalysis.CSharp
{
    public static class FxDiagnosticIds
    {
        private const string IdPrefix = "FX"; //// TODO apparently it's not good to use 2-character prefixes

        public const string BaseLowercaseTypeNameAnalyzerDiagnosticId = $"{FxDiagnosticIds.IdPrefix}0001";
    }
}
