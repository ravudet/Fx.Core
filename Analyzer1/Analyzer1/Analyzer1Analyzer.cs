namespace Analyzer1
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer1Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer1";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            ////context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

            //// TODO i think you'll want to check these out for `assert.that`:
            ////SyntaxKind.PointerMemberAccessExpression
            ////SyntaxKind.ConditionalAccessExpression
            ////SyntaxKind.SimpleMemberAccessExpression
            //// this is the expression type: https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.memberaccessexpressionsyntax?view=roslyn-dotnet-5.0.0

            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
            ////context.RegisterSyntaxNodeAction(AnalyzeNode2, SyntaxKind.SimpleMemberAccessExpression);
            context.RegisterSyntaxNodeAction(AnalyzeNode3, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;
            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                return;
            }

            // Perform data flow analysis on the local declaration.
            DataFlowAnalysis dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

            // Retrieve the local symbol for each variable in the local declaration
            // and ensure that it is not written outside of the data flow analysis region.
            VariableDeclaratorSyntax variable = localDeclaration.Declaration.Variables.Single();
            ISymbol variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable, context.CancellationToken);
            if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), localDeclaration.Declaration.Variables.First().Identifier.ValueText));
        }

        private void AnalyzeNode3(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;

            var symbol = context.SemanticModel.GetSymbolInfo(invocationExpression).Symbol as IMethodSymbol;
            if (symbol == null)
            {
                return;
            }

            if (!symbol.IsStatic)
            {
                return;
            }

            if (!symbol.ReceiverType.MetadataName.Equals("Assert"))
            {
                return;
            }

            var assemblyIdentity = symbol.ContainingAssembly.Identity;

            if (!assemblyIdentity.Name.Equals("Microsoft.VisualStudio.TestPlatform.TestFramework"))
            {
                return;
            }

            if (!assemblyIdentity.HasPublicKey)
            {
                return;
            }

            if (!assemblyIdentity.PublicKey.SequenceEqual(StringToByteArrayFastest("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")))
            {
                return;
            }

            //// TODO this also catches `assert.that` when it shouldn't (i think because `that.istrue` is a static invocation that has a receiver type of `assert` (even though the receiver isn't static)
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), symbol.MetadataName));
        }

        private void AnalyzeNode2(SyntaxNodeAnalysisContext context)
        {
            var memberAccessExpression = (MemberAccessExpressionSyntax)context.Node;

            var containingObject = memberAccessExpression.Expression;

            if (containingObject.Kind() == SyntaxKind.IdentifierName)
            {
                var identifierSyntax = (IdentifierNameSyntax)containingObject;
                var identifier = identifierSyntax.Identifier;

                ////var foo = memberAccessExpression.Name;

                var typeInfo = context.SemanticModel.GetTypeInfo(identifierSyntax);

                var type = typeInfo.Type;

                if (type.Name.Equals("Assert"))
                {
                    var assemblyIdentity = type.ContainingAssembly.Identity;
                    if (assemblyIdentity.Name.Equals("Microsoft.VisualStudio.TestPlatform.TestFramework") &&
                        assemblyIdentity.HasPublicKey &&
                        ////assemblyIdentity.PublicKeyToken.SequenceEqual(StringToByteArrayFastest("b03f5f7f11d50a3a"))
                        assemblyIdentity.PublicKey.SequenceEqual(StringToByteArrayFastest("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293"))
                        )
                    {
                        //// TODO check that it's a method access
                        var members = context.SemanticModel.GetMemberGroup(memberAccessExpression);
                    }
                }







                //// TODO make each rule configurably enabled


                //// TODO if they don't depend on unit testing, there is no error
                //// TODO if they don't have fx.test, add it as a nuget package
                ////context.SemanticModel.Compilation.ReferencedAssemblyNames.First()
                //// TODO if fx.test doesn't support it, add a local extensions file
            }
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
