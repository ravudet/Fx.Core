namespace Microsoft.CodeAnalysis.Testing
{
    using System.Linq;
    using System.Text;

    public static class DiagnosticResultExtensions
    {
        public static string ToCompilerError(this DiagnosticResult diagnosticResult)
        {
            var compilerError = new StringBuilder();
            if (diagnosticResult.Spans.TryFirst(out var location))
            {
                compilerError.Append($"({location.Span.StartLinePosition.Line + 1},{location.Span.StartLinePosition.Character + 1}): ");
            }

            compilerError.Append($"{diagnosticResult.Severity.ToString().ToLowerInvariant()} {diagnosticResult.Id}:");

            return compilerError.ToString();
        }
    }
}
