namespace Microsoft.CodeAnalysis.Diagnostics
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;

    public abstract class BaseTaskInterfaceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            FxCoreDiagnosticIds.BaseTaskInterfaceAnalyzerDiagnosticId,
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceTitle), 
                Resources.ResourceManager, 
                typeof(Resources)),
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceMessageFormat), 
                Resources.ResourceManager, 
                typeof(Resources)), 
            Categories.Design, //// TODO is this the correct category
            DiagnosticSeverity.Warning,
            true, //// TODO should be false
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceDescription), 
                Resources.ResourceManager, 
                typeof(Resources)),
            "TODO helplinkuri");

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
            var config = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Compilation.SyntaxTrees.First());
            var configResult = config.TryGetValue("ravudet", out var configValue);

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
                    ////message = $"'{configValue}'";
                    var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name, message);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
