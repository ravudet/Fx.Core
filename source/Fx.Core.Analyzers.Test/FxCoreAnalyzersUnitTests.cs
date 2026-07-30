using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using VerifyCS2 = Fx.Core.Analyzers.Test.Test<
    Microsoft.CodeAnalysis.CSharp.TaskInterfaceAnalyzer,
    Microsoft.CodeAnalysis.CodeFixes.TaskInterfaceCodeFixProvider>;

namespace Fx.Core.Analyzers.Test
{
    [TestClass]
    public class FxCoreAnalyzersUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            var tester = new VerifyCS2
            {
                TestCode = test,
                //// TODO the below lines were added to the template
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(
"""
#is_global = true

root = true

[*]
ravudet = true
"""))
                    }
                }
                //// TODO the above lines were added to the template
            };

            await tester.RunAsync(CancellationToken.None);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
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
        public static class Foo
        {
            public static async Task<int> {|#0:DoWork|}()
            {
                return await Task.FromResult(3).ConfigureAwait(false);
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS2.Diagnostic(FxCoreDiagnosticIds.BaseTaskInterfaceAnalyzerDiagnosticId).WithLocation(0).WithArguments("DoWork", string.Empty); //.WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Error);

            ////await VerifyCS.VerifyAnalyzerAsync(test, expected);
            ////await VerifyCS.VerifyCodeFixAsync(test, expected, test);

            var tester = new VerifyCS2
            {
                TestCode = test,
                //// TODO the below lines were added to the template
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(
"""
#is_global = true

root = true

[*]
ravudet = true

#dotnet_diagnostic.FXCORE0001.severity = none
"""))
                    },
                },
                //// TODO the above lines were added to the template
            };

            tester.ExpectedDiagnostics.Add(expected);

            await tester.RunAsync(CancellationToken.None);
        }

        [TestMethod]
        public async Task TestMethod3()
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
        public static class Foo
        {
            public static async Task<int> {|#0:DoWork|}()
            {
                return await Task.FromResult(3).ConfigureAwait(false);
            }
        }

        public interface IFoo<out T>
        {
        }
    }";

            var expected = VerifyCS2.Diagnostic(FxCoreDiagnosticIds.BaseTaskInterfaceAnalyzerDiagnosticId).WithLocation(0).WithArguments("DoWork", string.Empty); //.WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Error);

            ////await VerifyCS.VerifyAnalyzerAsync(test, expected);
            ////await VerifyCS.VerifyCodeFixAsync(test, expected, test);

            var tester = new VerifyCS2
            {
                TestCode = test,
                //// TODO the below lines were added to the template
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(
"""
#is_global = true

root = true

[*]
ravudet = true

#dotnet_diagnostic.FXCORE0001.severity = none
"""))
                    },
                },
                //// TODO the above lines were added to the template
            };


            //// TODO package needs to be "restored" before this will work...i'm not really sure what that means, but i got it working previously by creating the package with the right version number, installing that package in one of my projects (so that it ended up in the local package cache), and then running the test
            ////tester.ReferenceAssemblies = tester.ReferenceAssemblies.AddPackages(System.Collections.Immutable.ImmutableArray.Create(new Microsoft.CodeAnalysis.Testing.PackageIdentity("Fx.Core", "3.0.0")));

            var fxCorePath = GetPathWithoutExtension(FullPath(typeof(System.Threading.Tasks.ITask<>).Assembly));
            tester.ReferenceAssemblies = tester.ReferenceAssemblies.AddAssemblies([fxCorePath]);

            tester.ExpectedDiagnostics.Add(expected);

            await tester.RunAsync(CancellationToken.None);
        }

        private static string GetPathWithoutExtension(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            if (directory == null)
            {
                return fileNameWithoutExtension;
            }

            return Path.Combine(directory, fileNameWithoutExtension);
        }

        private static string FullPath(System.Reflection.Assembly assembly)
        {
            var uri = new System.Uri(assembly.CodeBase);
            return uri.LocalPath;
        }
    }
}
