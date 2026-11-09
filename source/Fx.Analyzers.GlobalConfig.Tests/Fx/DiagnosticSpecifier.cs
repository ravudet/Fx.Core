namespace Fx
{
    using Microsoft.CodeAnalysis.Testing;

    public sealed class DiagnosticSpecifier //// TODO put this in a shared library
    {
        public DiagnosticSpecifier(string testName, string resourceName, DiagnosticResult diagnosticResult)
        {
            TestName = testName; //// TODO i hate this name; "skuName" maybe is better?
            ResourceName = resourceName;
            DiagnosticResult = diagnosticResult;
        }

        public string TestName { get; }
        public string ResourceName { get; }
        public DiagnosticResult DiagnosticResult { get; }
    }
}
