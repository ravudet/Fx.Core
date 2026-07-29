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
            Categories.Usage,
            DiagnosticSeverity.Warning,
            false,
            new LocalizableResourceString(
                nameof(Resources.TaskInterfaceDescription), 
                Resources.ResourceManager, 
                typeof(Resources)),
            "https://github.com/ravudet/Fx.Core/blob/main/source/Fx.Core.Analyzers/Microsoft/CodeAnalysis/CSharp/TaskInterfaceAnalyzer.md");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            /*var config = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Compilation.SyntaxTrees.First());
            var configResult = config.TryGetValue("ravudet", out var configValue);*/

            //// TODO you should also find cases like async funcs


            var myType = context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ITask"); //// TODO i don't know how you're supposed to get the generic
            var message = string.Empty;
            if (myType is null)
            {
                // message = "The 'Fx.Core' package (which contains 'ITask<T>') is also missing.";
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
