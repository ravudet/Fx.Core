namespace Microsoft.CodeAnalysis.CodeFixes
{
    using System.Composition;

    using Microsoft.CodeAnalysis;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LowercaseTypeNameCodeFixProvider)), Shared]
    public class LowercaseTypeNameCodeFixProvider : BaseLowercaseTypeNameCodeFixProvider
    {
    }
}
