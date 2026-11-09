namespace Fx
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;

    public static class DiagnosticSpecifiers
    {
        public static DiagnosticSpecifier FX0001 { get; } = new DiagnosticSpecifier(
            nameof(FX0001),
            $"_resources.{nameof(FX0001)}.cs",
            DiagnosticResult.CompilerError("FX0001")
                .WithSeverity(DiagnosticSeverity.Error)
                .WithSpan(3, 25, 3, 28)
                .WithArguments("Foo"));
    }
}
