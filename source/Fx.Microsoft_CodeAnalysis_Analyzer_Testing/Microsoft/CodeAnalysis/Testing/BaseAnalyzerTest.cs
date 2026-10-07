namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    public static class AnalyzerTest
    {
        public static AnalyzerTest<TVerifier> Create<TVerifier>(
            string testCode, 
            CompilationOptions compilationOptions,
            ParseOptions parseOptions,
            IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers,
            string defaultFileExt,
            string language)
            where TVerifier : IVerifier, new()
        {
            return BaseAnalyzerTest<TVerifier>.Create(testCode, compilationOptions, parseOptions, diagnosticAnalyzers, defaultFileExt, language);
        }

        public static class Csharp
        {
            public static AnalyzerTest<TVerifier> Create<TVerifier>(string testCode)
                where TVerifier : IVerifier, new()
            {
                return AnalyzerTest.Csharp.Create<TVerifier>(testCode, AnalyzerTest.Csharp.Settings.Default);
            }

            public static AnalyzerTest<TVerifier> Create<TVerifier>(string testCode, AnalyzerTest.Csharp.Settings settings)
                where TVerifier : IVerifier, new()
            {
                return AnalyzerTest.Create<TVerifier>(
                    testCode, 
                    settings.CompilationOptions, 
                    settings.ParseOptions, 
                    settings.DiagnosticAnalyzers, 
                    "cs",
                    LanguageNames.CSharp);
            }

            public sealed class Settings
            {
                public Settings(CSharpCompilationOptions compilationOptions, CSharpParseOptions parseOptions, IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers)
                {
                    CompilationOptions = compilationOptions;
                    ParseOptions = parseOptions;
                    DiagnosticAnalyzers = diagnosticAnalyzers;
                }

                public static Settings Default { get; } = new AnalyzerTest.Csharp.Settings.Builder().Build();

                public CSharpCompilationOptions CompilationOptions { get; }

                public CSharpParseOptions ParseOptions { get; }

                public IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { get; }

                public sealed class Builder
                {
                    public CSharpCompilationOptions CompilationOptions { get; set; } = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);

                    public CSharpParseOptions ParseOptions { get; set; } = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);

                    public IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { get; set; } = Enumerable.Empty<DiagnosticAnalyzer>();

                    public AnalyzerTest.Csharp.Settings Build()
                    {
                        return new AnalyzerTest.Csharp.Settings(
                            this.CompilationOptions,
                            this.ParseOptions,
                            this.DiagnosticAnalyzers);
                    }
                }
            }
        }
    }

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
