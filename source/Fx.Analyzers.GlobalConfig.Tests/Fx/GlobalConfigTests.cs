namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalConfigTests
    {
        [TestMethod]
        public async Task Test()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class Foo
        {   
            public class Bar
            {
            }
        }
    }";

            var tester = new CustomAnalyzerTest()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(
"""
#is_global = true

root = true

[*]
dotnet_diagnostic.CA1034.severity = warning
"""))
                    }
                }
            };

            tester.ExpectedDiagnostics.Add(new DiagnosticResult("CA1034", DiagnosticSeverity.Warning));

            await tester.RunAsync().ConfigureAwait(false);
        }
    }


    public sealed class CustomAnalyzerTest : AnalyzerTest<MSTestVerifier>
    {
        private static readonly LanguageVersion DefaultLanguageVersion =
            Enum.TryParse("Default", out LanguageVersion version) ? version : LanguageVersion.CSharp6;

        ///public override Type SyntaxKindType => typeof(SyntaxKind);

        public override string Language => LanguageNames.CSharp;

        protected override string DefaultFileExt => "cs";

        protected override CompilationOptions CreateCompilationOptions()
        {
            return new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);
        }

        protected override ParseOptions CreateParseOptions()
        {
            return new CSharpParseOptions(DefaultLanguageVersion, DocumentationMode.Diagnose);
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            /*var foo = Path.GetDirectoryName(typeof(Microsoft.NetFramework.Analyzers.TypesShouldNotExtendCertainBaseTypesAnalyzer).Assembly.Location);
            foo = Path.Combine(foo, "Microsoft.CodeAnalysis.CSharp.CodeStyle.dll");*/

            var paths = new[]
            {
                typeof(Microsoft.NetFramework.Analyzers.TypesShouldNotExtendCertainBaseTypesAnalyzer).Assembly.Location,
                //foo,
                ////typeof(Microsoft.CodeAnalysis.Completion.CompletionTags).Assembly.Location,
                //typeof(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext).Assembly.Location, // TODO foo depends on this
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.dll",
            };

            ////var assembly = Assembly.Load(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll");

            /*var type = typeof(Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.DiagnosticAnalyzerAttributeAnalyzer);
            var another = (DiagnosticAnalyzer)Activator.CreateInstance(type)!;*/

            return paths.SelectMany(path => Load(path))/*.Append(another).ToImmutableArray()*/;
        }

        public static IEnumerable<DiagnosticAnalyzer> Load(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            var types = assembly.GetTypes();
            var analyzers = types
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract);

            var publicAnalyzers = analyzers.Where(analyzer => analyzer.IsPublic);

            return analyzers.Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
        }
    }

}
