namespace Fx
{
    using System.Collections.Generic;
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

        private static IEnumerable<DiagnosticAnalyzer> DefaultAnalyzers { get; } = DiagnosticAnalyzers.Extensions.Load(DiagnosticAnalyzers.Extensions.DefaultAssembly());

        private static async Task<string> GetManifestResourceString(string resourceName)
        {
            //// TODO do you want to use Assembly.GetCallingAssembly?

            return await typeof(GlobalConfigTests).Assembly.GetManifestResourceString(resourceName).ConfigureAwait(false);
        }

        private static async Task<string> GetTestString([CallerMemberName] string testName = "")
        {
            //// TODO do you like this method name?
            return await GlobalConfigTests.GetManifestResourceString($"_resources.{testName}.cs").ConfigureAwait(false);
        }

        private static async Task<string> GetEditorConfigString()
        {
            return await GetManifestResourceString("Fx.GlobalConfig.globalconfig").ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CA1510()
        {
            var test = await GetTestString().ConfigureAwait(false); //// TODO do you like this variable name?
            var editorConfig = await GetEditorConfigString().ConfigureAwait(false); //// TODO do you like this variable name?

            var tester = new CsharpAnalyzerTest<MSTestVerifier>()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    },
                },
                DiagnosticAnalyzers = GlobalConfigTests.DefaultAnalyzers,
            };

            tester.ExpectedDiagnostics.Add(new DiagnosticResult("CA1510", DiagnosticSeverity.Error).WithSpan(13, 13, 16, 14).WithSpan(13, 17, 13, 27).WithArguments("ArgumentNullException", "ThrowIfNull"));

            await tester.RunAsync().ConfigureAwait(false);
        }
    }
}
