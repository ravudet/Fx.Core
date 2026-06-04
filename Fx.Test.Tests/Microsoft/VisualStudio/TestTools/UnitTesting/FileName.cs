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




            //// TODO even though you've ide-only triggering on build, some of them (like file header) aren't triggering; they *do* trigger on `dotnet build` though, which makes me think that you're just not loading that analyzer somehow //// TODO and you confirmed that a change to 1052 in the `.editorconfig` was reflected in the compiler diagnostics





            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64");

            //Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.314");

            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.314\Containers\containerize");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.314\DotnetTools\dotnet-watch\9.0.314-servicing.26230.9\tools\net9.0\any");

            //Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.301");

            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.301\Containers\containerize");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.301\DotnetTools\dotnet-watch\9.0.301-servicing.25269.4\tools\net9.0\any");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\amd64");

            //Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.102");

            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\9.0.102\Containers\containerize");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\Microsoft.NET\assembly\GAC_32\MSBuild\v4.0_4.0.0.0__b03f5f7f11d50a3a");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\WinSxS\wow64_msbuild_b03f5f7f11d50a3a_4.0.15912.0_none_07ea43e35ad4fd3b");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_4.0.0.0__b03f5f7f11d50a3a");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319");
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterMSBuildPath(@"C:\Windows\WinSxS\amd64_msbuild_b03f5f7f11d50a3a_4.0.15912.0_none_de1bfcc9998a681e");

            

            var instance = Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();


            ////Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
            await DoWork();
            
        }

        public static async Task DoWork()
        {
            await OpenSolution();
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

        public static async Task OpenSolution()
        {
            // TODO this has to be called before the other types are loaded by the runtime, so it needs to be outside of the method (ostensibly)
            ////

            var solutionPath = @"C:\github\OddTrotter\Fx.Core\Fx.Core.sln";
            //// TODO package `Microsoft.CodeAnalysis.Workspaces.MSBuild` actually depends on `Microsoft.Build`
            var workspace = MSBuildWorkspace.Create();

            /*var loader = new MSBuildProjectLoader(workspace);
            var solutionInfo = await loader.LoadSolutionInfoAsync(solutionPath);


            var projectInfo = await loader.LoadProjectInfoAsync(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Fx.Test.Tests.csproj");*/

            var solution = await workspace.OpenSolutionAsync(solutionPath);

            var project = solution.Projects.Where(project => project.Name == "ClassLibrary1").First();

            solution = solution.WithProjectCompilationOptions(project.Id, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            project = solution.Projects.Where(project => project.Name == "ClassLibrary1").First();

            var compilation = await project.GetCompilationAsync();
            var withAnalyzers = compilation.WithAnalyzers(LoadAll());
            //var diagnostics = compilation.GetDiagnostics();
            var diagnostics = await withAnalyzers.GetAllDiagnosticsAsync();
        }

        public static Project CreateProjectWithEditorConfig(
        string code,
        string editorConfigPath,
        string projectName = "TestProject")
        {
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
