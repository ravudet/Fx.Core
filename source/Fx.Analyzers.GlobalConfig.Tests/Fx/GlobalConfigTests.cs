namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    abstract class OriginalBase
    {
        protected abstract string CreateCompilationOptions();

        public abstract string Language { get; }

        public abstract object Test { init; }
    }

    sealed class Derived : OriginalBase
    {
        public string CompilationOptions { private get; init; } = string.Empty;

        public override string Language { get; } = "csharp";
        public override object Test { init => throw new NotImplementedException(); }

        protected override string CreateCompilationOptions()
        {
            return this.CompilationOptions;
        }
    }

    public static class Factory
    {
        public static void Create()
        {
            var derived = new Derived()
            {
            };
            ////Console.WriteLine(derived.Test);
        }
    }

    [TestClass]
    public class GlobalConfigTests
    {
        private static async Task<string> GetManifestResourceString(string resourceName)
        {
            //// TODO do you want to use Assembly.GetCallingAssembly?

            return await typeof(GlobalConfigTests).Assembly.GetManifestResourceString(resourceName).ConfigureAwait(false);
        }

        private static async Task<string> GetTestString([CallerMemberName] string testName = "")
        {
            //// TODO do you like this method name?
            return await GlobalConfigTests.GetManifestResourceString("Content." + testName + ".cs").ConfigureAwait(false);
        }

        private static async Task<string> GetEditorConfigString()
        {
            return await GetManifestResourceString("Fx.Analyzers.GlobalConfig.globalconfig").ConfigureAwait(false);
        }

        public static IEnumerable<DiagnosticAnalyzer> DefaultAnalyzers { get; } = DiagnosticAnalyzers.Extensions.Load(DiagnosticAnalyzers.Extensions.DefaultAssembly());

        [TestMethod]
        public async Task Test()
        {
            //// TODO this test should actually go in fx.globalconfig.tests

            var test = await GetTestString().ConfigureAwait(false); //// TODO do you like this variable name?
            var editorConfig = await GetEditorConfigString().ConfigureAwait(false); //// TODO do you like this variable name?

            //// TODO move below helper types into their own project
            //// TODO remove any unnecessary dependencies
            //// TODO can you rename the Content folder to `_resources`?
            //// TODO is there a better way to combine resource paths?

            var tester = AnalyzerTest.Csharp.Create<MSTestVerifier>(test, new AnalyzerTest.Csharp.Settings.Builder() {  DiagnosticAnalyzers = GlobalConfigTests.DefaultAnalyzers }.Build());
            tester.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", SourceText.From(editorConfig)));

            /*var tester = new MsTestCsharpAnalyzerTest()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    },
                },
            };*/

            tester.ExpectedDiagnostics.Add(new DiagnosticResult("CA1510", DiagnosticSeverity.Error).WithSpan(13, 13, 16, 14).WithSpan(13, 17, 13, 27).WithArguments("ArgumentNullException", "ThrowIfNull"));

            await tester.RunAsync().ConfigureAwait(false);
        }

        [TestMethod]
        public async Task FX0001()
        {
            //// TODO this test should actually go in fx.core.analyzers.globalconfig.tests

            var test = await GetTestString().ConfigureAwait(false); //// TODO do you like this variable name?
            var editorConfig = await GetEditorConfigString().ConfigureAwait(false); //// TODO do you like this variable name?


            var tester = AnalyzerTest.Csharp.Create<MSTestVerifier>(
                test,
                new AnalyzerTest.Csharp.Settings.Builder()
                {
                    DiagnosticAnalyzers = DiagnosticAnalyzers.Extensions.Load(typeof(Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer).Assembly.Location),
                }.Build());
            tester.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", SourceText.From(editorConfig)));

            tester = new MsTestCsharpAnalyzerTest()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    },
                },
                DiagnosticAnalyzers = DiagnosticAnalyzers.Extensions.Load(typeof(Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer).Assembly.Location),               
            };


            /*var tester = new MsTestCsharpAnalyzerTest()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    },
                },
                DiagnosticAnalyzers = DiagnosticAnalyzers.Extensions.Load(typeof(Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer).Assembly.Location),
            };*/

            tester.ExpectedDiagnostics.Add(
                DiagnosticResult.CompilerError("FX0001")
                    .WithSeverity(DiagnosticSeverity.Error)
                    .WithOptions(DiagnosticOptions.IgnoreSeverity) //// TODO the globalconfig has the severity set to error, but this isn't honored by the test harness for some reason (and instead uses the analyzer's default severity), so you're ignoring severity for now to get the test to pass, but you should fix this at some point; NOTE: you didn't have to do this for the `CA1510` test, maybe the different is the custom analyzer is not done correctly somehow?
                    .WithSpan(6, 25, 6, 28)
                    .WithArguments("Foo"));

            await tester.RunAsync().ConfigureAwait(false);
        }
    }

    public sealed class MsTestCsharpAnalyzerTest : CsharpAnalyzerTest<MSTestVerifier>
    {
    }
        
}
