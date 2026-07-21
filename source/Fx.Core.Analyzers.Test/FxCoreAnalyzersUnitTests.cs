using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using VerifyCS = Fx.Core.Analyzers.Test.CSharpCodeFixVerifier<
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

            await VerifyCS.VerifyAnalyzerAsync(test);
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

            var expected = VerifyCS.Diagnostic(DiagnosticIds.BaseTaskInterfaceAnalyzerDiagnosticId).WithLocation(0).WithArguments("DoWork", string.Empty);

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
            ////await VerifyCS.VerifyCodeFixAsync(test, expected, test);
        }
    }
}
