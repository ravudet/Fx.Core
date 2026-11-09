namespace Fx
{
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalConfigTests
    {
        //// TODO do any of these helper methods need to be factored out and shared with the `...Analzyers.Tests` projects?

        private static async Task<string> GetManifestResourceString(string resourceName)
        {
            //// TODO do you want to use Assembly.GetCallingAssembly?

            return await typeof(GlobalConfigTests).Assembly.GetManifestResourceString(resourceName).ConfigureAwait(false);
        }

        private static async Task<string> GetEditorConfigString()
        {
            //// TODO can you compute this string?
            return await GetManifestResourceString("Fx.Analyzers.GlobalConfig.globalconfig").ConfigureAwait(false);
        }

        [TestMethod]
        public async Task FX0001()
        {
            var diagnosticSpecifier = DiagnosticSpecifiers.FX0001;

            var codeToAnalyze = await GetManifestResourceString(diagnosticSpecifier.ResourceName).ConfigureAwait(false);
            var editorConfigContents = await GetEditorConfigString().ConfigureAwait(false);

            var tester = new CsharpAnalyzerTest<MSTestVerifier>()
            {
                TestCode = codeToAnalyze,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfigContents)),
                    },
                },
                DiagnosticAnalyzers = DiagnosticAnalyzers.Extensions.Load(typeof(Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer).Assembly),               
            };

            tester.ExpectedDiagnostics.Add(diagnosticSpecifier.DiagnosticResult.WithOptions(DiagnosticOptions.IgnoreSeverity)); //// TODO the globalconfig has the severity set to error, but this isn't honored by the test harness for some reason (and instead uses the analyzer's default severity), so you're ignoring severity for now to get the test to pass, but you should fix this at some point; NOTE: you didn't have to do this for the `CA1510` test, maybe the different is the custom analyzer is not done correctly somehow?

            await tester.RunAsync().ConfigureAwait(false);
        }
    }
}
