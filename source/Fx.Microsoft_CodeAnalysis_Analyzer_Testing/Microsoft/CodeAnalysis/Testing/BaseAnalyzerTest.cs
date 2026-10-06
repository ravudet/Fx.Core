namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.Diagnostics;

    public sealed class BaseAnalyzerTest<TVerifier> : AnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
    {
        private BaseAnalyzerTest()
        {
        }

        private CompilationOptions CompilationOptions { get; } = BaseAnalyzerTest<TVerifier>.BuilderCompilationOptions;
        private static CompilationOptions BuilderCompilationOptions { get; set; }

        private ParseOptions ParseOptions { get; } = BaseAnalyzerTest<TVerifier>.BuilderParseOptions;
        private static ParseOptions BuilderParseOptions { get; set; }

        private IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { get; } = BaseAnalyzerTest<TVerifier>.BuilderDiagnosticAnalyzers;
        private static IEnumerable<DiagnosticAnalyzer> BuilderDiagnosticAnalyzers { get; set; }

        protected override string DefaultFileExt { get; } = BaseAnalyzerTest<TVerifier>.BuilderDefaultFileExt;
        private static string BuilderDefaultFileExt { get; set; }

        public override string Language { get; } = BaseAnalyzerTest<TVerifier>.BuilderLanguage;
        private static string BuilderLanguage { get; set; }

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

        public sealed class Builder
        {
            private static readonly object @lock = new object();

            public required CompilationOptions CompilationOptions { get; set; }

            public required ParseOptions ParseOptions { get; set; }

            public required IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { get; set; }

            public required string DefaultFileExt { get; set; }

            public required string Language { get; set; }

            public BaseAnalyzerTest<TVerifier> Build()
            {
                //// TODO document why this works
                lock (@lock)
                {
                    BaseAnalyzerTest<TVerifier>.BuilderCompilationOptions = this.CompilationOptions;
                    BaseAnalyzerTest<TVerifier>.BuilderParseOptions = this.ParseOptions;
                    BaseAnalyzerTest<TVerifier>.BuilderDiagnosticAnalyzers = this.DiagnosticAnalyzers;
                    BaseAnalyzerTest<TVerifier>.BuilderDefaultFileExt = this.DefaultFileExt;
                    BaseAnalyzerTest<TVerifier>.BuilderLanguage = this.Language;

                    return new BaseAnalyzerTest<TVerifier>();
                }
            }
        }
    }
}
