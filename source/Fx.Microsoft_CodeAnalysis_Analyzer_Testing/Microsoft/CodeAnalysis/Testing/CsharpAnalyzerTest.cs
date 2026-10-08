namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    public class CsharpAnalyzerTest<TVerifier> : BaseAnalyzerTest<TVerifier, CSharpCompilationOptions, CSharpParseOptions>
        where TVerifier : IVerifier, new()
    {
        public override CSharpCompilationOptions CompilationOptions { protected get; init; } = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);

        public override CSharpParseOptions ParseOptions { protected get; init; } = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);

        public override IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { protected get; init; } = Enumerable.Empty<DiagnosticAnalyzer>();

        public override string Language { get; } = LanguageNames.CSharp; //// TODO property, or return constant?

        protected override string DefaultFileExt { get; } = "cs"; //// TODO property, or return constant?
    }
}
