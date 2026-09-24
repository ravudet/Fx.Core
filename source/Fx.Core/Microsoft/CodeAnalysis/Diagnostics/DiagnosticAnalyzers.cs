namespace Microsoft.CodeAnalysis.Diagnostics
{
    public sealed class DiagnosticAnalyzers
    {
        private DiagnosticAnalyzers()
        {
        }

        public static DiagnosticAnalyzers Extensions { get; } = new DiagnosticAnalyzers();
    }
}
