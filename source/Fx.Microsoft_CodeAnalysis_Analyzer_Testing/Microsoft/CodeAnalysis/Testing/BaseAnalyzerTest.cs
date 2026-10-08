namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    public abstract class BaseAnalyzerTest<TVerifier, TCompilationOptions, TParseOptions> : AnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
        where TCompilationOptions : CompilationOptions
        where TParseOptions : ParseOptions
    {
        public abstract TCompilationOptions CompilationOptions { protected get; init; } //// TODO can you do better than `protected`? `internal` doesn't do anything
        public abstract TParseOptions ParseOptions { protected get; init; }
        public abstract IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { protected get; init; }

        protected override CompilationOptions CreateCompilationOptions()
        {
            return this.CompilationOptions;
        }

        protected override ParseOptions CreateParseOptions()
        {
            return this.ParseOptions;
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            return this.DiagnosticAnalyzers;
        }
    }

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
