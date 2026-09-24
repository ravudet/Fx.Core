namespace Microsoft.CodeAnalysis.Testing
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.CodeAnalysis.CSharp;

    public class CsharpAnalyzerTest<TVerifier> : BaseAnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
    {
        [SetsRequiredMembers]
        public CsharpAnalyzerTest()
        {
            //// TODO put setting default analyzers in base type

            base.CompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);
            base.ParseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);
            base.DiagnosticAnalyzers = GlobalConfigTests.DefaultAnalyzers;
        }

        public override string Language { get; } = LanguageNames.CSharp;

        protected override string DefaultFileExt { get; } = "cs";
    }
}
