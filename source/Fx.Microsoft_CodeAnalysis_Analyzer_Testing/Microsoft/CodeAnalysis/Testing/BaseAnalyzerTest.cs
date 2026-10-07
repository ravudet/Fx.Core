namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.Diagnostics;

    public sealed class BaseAnalyzerTest<TVerifier> : AnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
    {
        private static readonly object @lock = new object();

        public static BaseAnalyzerTest<TVerifier> Create(string testCode, 
            CompilationOptions compilationOptions,
            ParseOptions parseOptions,
            IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers, //// TODO this should default to empty...
            string defaultFileExt,
            string language)
        {
            // This is a very peculiar way to instantiate a type, so let's explain the motivation, and then explain how the
            // provided code accomplishes the goal.
            //
            // The objective is to allow instantiating a `AnalyzerTest<TVerifier>` where the caller can provide the
            // appropriate `CompilationOptions`, `ParseOptions`, `DiagnosticAnalyzer`s, `DefaultFileExt`, and `Language`. As
            // designed, `AnalyzerTest<TVerifier>` requires inheritance for providing these values by making them
            // `abstract`. The issue with this design is that most of these values (`CompilationOptions`, `ParseOptions`, and `DiagnosticAnalyzer`s) are highly configurable, and so they should not require a new derived type for every combination of configurations. 

            // https://learn.microsoft.com/en-us/answers/questions/462106/in-what-order-do-things-happen
            // https://giannisakritidis.com/blog/Instantiation-And-Initialization-Order/

            //// TODO document why this works
            lock (BaseAnalyzerTest<TVerifier>.@lock)
            {
                BaseAnalyzerTest<TVerifier>.BuilderCompilationOptions = compilationOptions;
                BaseAnalyzerTest<TVerifier>.BuilderParseOptions = parseOptions;
                BaseAnalyzerTest<TVerifier>.BuilderDiagnosticAnalyzers = diagnosticAnalyzers;
                BaseAnalyzerTest<TVerifier>.BuilderDefaultFileExt = defaultFileExt;
                BaseAnalyzerTest<TVerifier>.BuilderLanguage = language;

                return new BaseAnalyzerTest<TVerifier>()
                {
                    TestCode = testCode,
                };
            }
        }

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
    }
}
