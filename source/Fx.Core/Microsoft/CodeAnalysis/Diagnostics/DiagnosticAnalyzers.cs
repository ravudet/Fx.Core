namespace Microsoft.CodeAnalysis.Diagnostics
{
    public sealed class DiagnosticAnalyzers //// TODO i like this pattern for "static extension methods", but i don't know that the property name "extensions" is the best...
    {
        private DiagnosticAnalyzers()
        {
        }

        public static DiagnosticAnalyzers Extensions { get; } = new DiagnosticAnalyzers();
    }
}
