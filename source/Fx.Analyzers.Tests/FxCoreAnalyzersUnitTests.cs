using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using VerifyCS2 = Fx.Core.Analyzers.Test.Test<
    Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer,
    Microsoft.CodeAnalysis.CodeFixes.LowercaseTypeNameCodeFixProvider>;

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
        class {|#0:TypeName|}
        {   
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

            var expected = VerifyCS2.Diagnostic(FxDiagnosticIds.BaseLowercaseTypeNameAnalyzerDiagnosticId).WithLocation(0).WithArguments("TypeName");
            ////await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);





            var tester = new VerifyCS2
            {
                TestCode = test,
                FixedCode = fixtest,
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

            tester.ExpectedDiagnostics.Add(expected);
            await tester.RunAsync(CancellationToken.None);
        }
    }
}
