namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;

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
}
