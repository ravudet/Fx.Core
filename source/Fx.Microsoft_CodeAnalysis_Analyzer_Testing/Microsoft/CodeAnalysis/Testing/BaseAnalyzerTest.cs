namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.Diagnostics;

    public abstract class BaseAnalyzerTest<TVerifier> : AnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
    {
        public required CompilationOptions CompilationOptions { private get; init; }

        public required ParseOptions ParseOptions { private get; init; }

        public required IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { private get; init; }

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
