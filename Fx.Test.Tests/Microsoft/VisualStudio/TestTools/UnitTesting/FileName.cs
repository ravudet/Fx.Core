// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Foo;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.CodeAnalysis.Text;

    internal class FileName
    {
        public static void DoWork()
        {
            if (true)
            {
            }

            Demo.DoWork();
            Console.WriteLine("asdF");
        }
    }


    [TestClass]
    public class EditorConfigTests2
    {
        [TestMethod]
        public async Task Foo2()
        {
            var config =
"""
root = true

[*.cs]

# IDE0003: Remove 'this' qualification
dotnet_diagnostic.IDE0003.severity = error
""";

            var code = @"
internal class C
{
    private int __x;

    void M()
    {
        this.__x = 1; // unnecessary 'this', should trigger IDE0003
    }
}
";

            var project = CreateProjectWithEditorConfig(code, @"C:\github\OddTrotter\Fx.Core\.editorconfig");

            var diagnostics = await RunAnalyzersAsync(
                project,
                LoadAll().ToArray());

            Assert.IsTrue(diagnostics.Any(d => d.Id == "IDE0003"));
        }

        public static ImmutableArray<DiagnosticAnalyzer> LoadAll()
        {
            var assembly = Assembly.LoadFrom(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll");
            ////var assembly = Assembly.Load(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll");

            return assembly
                .GetTypes()
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!)
                .ToImmutableArray();
        }

        public static Project CreateProjectWithEditorConfig(
        string code,
        string editorConfigPath,
        string projectName = "TestProject")
        {
            
            MSBuildWorkspace.Create().OpenSolutionAsync()

            var workspace = new AdhocWorkspace();

            var editorConfigText = SourceText.From(File.ReadAllText(editorConfigPath));


            var projectId = ProjectId.CreateNewId();
            var solution = workspace.CurrentSolution
                .AddProject(projectId, projectName, projectName, LanguageNames.CSharp)
                .WithProjectCompilationOptions(
                    projectId,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var project = solution.GetProject(projectId)!;

            // Add references (you can add more as needed)
            project = project.AddMetadataReference(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            // Add source document
            project = project.AddDocument("Test.cs", SourceText.From(code)).Project;

            // Add .editorconfig as analyzer config document

            var reference = new AnalyzerImageReference(LoadAll());

            project = project.AddAnalyzerConfigDocument(
                ".editorconfig",
                editorConfigText,
                filePath: Path.Combine(solution.FilePath, ".editorConfig")).Project;

            project = project.AddAnalyzerReference(reference);

            return project;
        }

        public static async Task<ImmutableArray<Diagnostic>> RunAnalyzersAsync(
            Project project,
            params DiagnosticAnalyzer[] analyzers)
        {

            var compilation = await project.GetCompilationAsync();

            var withAnalyzers = compilation!.WithAnalyzers(analyzers.ToImmutableArray());
            return await withAnalyzers.GetAllDiagnosticsAsync();
            //return await withAnalyzers.GetAnalyzerDiagnosticsAsync();
        }
    }
}
