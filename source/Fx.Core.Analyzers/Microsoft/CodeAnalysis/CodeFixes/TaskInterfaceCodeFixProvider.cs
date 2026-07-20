namespace Microsoft.CodeAnalysis.CodeFixes
{
    using System.Composition;

    using Microsoft.CodeAnalysis;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TaskInterfaceCodeFixProvider)), Shared]
    public class TaskInterfaceCodeFixProvider : BaseTaskInterfaceCodeFixProvider
    {
    }
}
