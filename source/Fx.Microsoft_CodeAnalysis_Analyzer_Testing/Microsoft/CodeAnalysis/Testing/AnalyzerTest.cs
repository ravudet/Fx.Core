namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    abstract class OriginalBase
    {
        protected abstract string CreateCompilationOptions();

        protected abstract string Language { get; }
    }

    abstract class Base : OriginalBase
    {
        public required string CompilationOptions { get; init; }

        protected override string CreateCompilationOptions()
        {
            return this.CompilationOptions;
        }
    }

    class Derived : Base
    {
        public required string DerivedCompilationOptions
        {
            get
            {
                return base.CompilationOptions;
            }
            init
            {
                base.CompilationOptions = value;
            }
        }

        protected override string Language { get; } = "csharp";
    }

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
}
