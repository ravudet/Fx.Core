namespace Fx
{
    using Microsoft.CodeAnalysis.Testing;

    public sealed class DiagnosticSpecifier //// TODO put this in a shared library
    {
        public DiagnosticSpecifier(string resourceName, DiagnosticResult diagnosticResult)
        {
            ResourceName = resourceName;
            DiagnosticResult = diagnosticResult;
        }

        public string ResourceName { get; }
        public DiagnosticResult DiagnosticResult { get; }
    }
}
