namespace Microsoft.CodeAnalysis.CSharp
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public abstract class BaseTaskInterfaceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticIds.BaseTaskInterfaceAnalyzerDiagnosticId,
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceTitle), 
                Resources.ResourceManager, 
                typeof(Resources)),
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceMessageFormat), 
                Resources.ResourceManager, 
                typeof(Resources)), 
            Categories.Design, 
            DiagnosticSeverity.Warning,
            true,
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceDescription), 
                Resources.ResourceManager, 
                typeof(Resources)));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            //// TODO you should also find cases like async funcs
            //// TODO you still need a codefix for this


            var myType = context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ITask"); //// TODO i don't know how you're supposed to get the generic
            var message = string.Empty;
            if (myType is null)
            {
                // message = "The 'Fx.Core' package which contains 'ITask<T>' is also missing.";
            }


            var methodSymbol = (IMethodSymbol)context.Symbol;

            var returnType = methodSymbol.ReturnType;
            var taskType = typeof(Task<>);

            var displayString = returnType.ToDisplayString();

            if (returnType.MetadataName == taskType.Name)
            {
                var returnTypeNamespace = returnType.ContainingNamespace.ToDisplayString();

                if (returnTypeNamespace == taskType.Namespace)
                {
                    var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name, message);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
