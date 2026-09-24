namespace Microsoft.CodeAnalysis.Testing
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    public class CsharpAnalyzerTest<TVerifier> : BaseAnalyzerTest<TVerifier>
        where TVerifier : IVerifier, new()
    {
        [SetsRequiredMembers]
        public CsharpAnalyzerTest()
        {
            //// TODO put setting default analyzers in base type

            base.CompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);
            base.ParseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose);
            //// TODO base.DiagnosticAnalyzers = GlobalConfigTests.DefaultAnalyzers;
            
            var test = new FakeAnalyzerTest()
            {
                TestState = new SolutionState(string.Empty, string.Empty, string.Empty, string.Empty),
            }
        }

        public override string Language { get; } = LanguageNames.CSharp;

        protected override string DefaultFileExt { get; } = "cs";

        private sealed class FakeAnalyzerTest : AnalyzerTest<TVerifier>
        {
            public override string Language { get; }

            protected override string DefaultFileExt { get; }

            protected override CompilationOptions CreateCompilationOptions()
            {
                throw new System.NotImplementedException();
            }

            protected override ParseOptions CreateParseOptions()
            {
                throw new System.NotImplementedException();
            }

            protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
