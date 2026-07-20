namespace Microsoft.CodeAnalysis.CSharp
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class LowercaseTypeNameAnalyzer : BaseLowercaseTypeNameAnalyzer
    {
    }
}
