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
                nameof(Resources.LowercaseTypeNameTitle), 
                Resources.ResourceManager, 
                typeof(Resources)),
            new LocalizableResourceString(
                nameof(Resources.LowercaseTypeNameMessageFormat), 
                Resources.ResourceManager, 
                typeof(Resources)), 
            Categories.Design, 
            DiagnosticSeverity.Warning,
            false,
            new LocalizableResourceString(
                nameof(Resources.LowercaseTypeNameDescription), 
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
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var methodSymbol = (IMethodSymbol)context.Symbol;

            var returnType = methodSymbol.ReturnType;
            var taskType = typeof(Task<>);

            if (returnType.Name == taskType.Name)
            {
                var returnTypeAssembly = returnType.ContainingAssembly;
                var taskTypeAssembly = taskType.Assembly;
                if (returnTypeAssembly.Name == taskTypeAssembly.GetName().Name)
                {
                }
            }

            // Find just those named type symbols with names containing lowercase letters.
            if (methodSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
