namespace Microsoft.CodeAnalysis.CSharp
{
    using System.Composition;
    
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LowercaseTypeNameCodeFixProvider)), Shared]
    public class LowercaseTypeNameCodeFixProvider : BaseLowercaseTypeNameCodeFixProvider
    {
    }
}
