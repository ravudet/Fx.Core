// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
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
        private static async Task<string> SetupSolution(
            string workingDirectory,
            Stream editorConfigContents,
            Stream projectContents,
            params (string FilePathRelativeToProjectRoot, Stream FileContents)[] files)
        {
            // returns the path to the solution file

            var repositoryPath = Path.Combine(workingDirectory, "solution");

            var editorConfigPath = Path.Combine(repositoryPath, ".editorconfig");
            await WriteAllContents(editorConfigPath, editorConfigContents).ConfigureAwait(false);

            var projectName = "Project";
            var projectRelativePath = Path.Combine(projectName, $"{projectName}.csproj");
            var projectPath = Path.Combine(repositoryPath, projectRelativePath);
            await WriteAllContents(projectPath, projectContents).ConfigureAwait(false);
            var projectRootPath = Path.GetDirectoryName(projectPath);
            foreach (var file in files)
            {
                var filePath = Path.Combine(projectRootPath, file.FilePathRelativeToProjectRoot);
                await WriteAllContents(filePath, file.FileContents).ConfigureAwait(false);
            }

            var solutionPath = Path.Combine(repositoryPath, "solution.sln");
            using (var solutionFile = CreateFileWrite(solutionPath))
            {
                using (var textWriter = new StreamWriter(solutionFile))
                {
                    await textWriter.WriteLineAsync(
$$"""
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.7.34031.279
MinimumVisualStudioVersion = 10.0.40219.1
Project("{00000000-0000-0000-0000-000000000001}") = "{{projectName}}", "{{projectRelativePath}}", "{10000000-0000-0000-0000-000000000000}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Solution Items", "Solution Items", "{02EA681E-C7D8-13C7-8484-4AC65E1B71E8}"
	ProjectSection(SolutionItems) = preProject
		.editorconfig = .editorconfig
	EndProjectSection
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{10000000-0000-0000-0000-000000000000}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{10000000-0000-0000-0000-000000000000}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{10000000-0000-0000-0000-000000000000}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{10000000-0000-0000-0000-000000000000}.Release|Any CPU.Build.0 = Release|Any CPU
    EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {938EAC26-C20C-48C0-B5F6-0B535D593D6B}
	EndGlobalSection
EndGlobal
"""
                        ).ConfigureAwait(false);
                }
            }

            return solutionPath;
        }

        private static async Task WriteAllContents(string filePath, Stream contents)
        {
            using (var file = CreateFileWrite(filePath))
            {
                await contents.CopyToAsync(file).ConfigureAwait(false);
            }
        }

        private static Stream CreateFileWrite(string filePath)
        {
            return OpenFileWrite(filePath, FileMode.CreateNew);
        }

        private static Stream OpenFileWrite(string filePath, FileMode fileMode)
        {
            return OpenFile(filePath, fileMode, FileAccess.Write, FileShare.None);
        }

        private static Stream OpenFile(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            while (true)
            {
                try
                {
                    return File.Open(filePath, fileMode, fileAccess, fileShare);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        public TestContext TestContext { get; set; } //// TODO does it have to be public?

        private static string CombineResourcePath(params string[] paths)
        {
            return Path.Combine(paths).Replace(Path.DirectorySeparatorChar, '.');
        }

        private const string EmbeddedResourceRootPath = "Content";

        [TestMethod]
        public async Task Foo3()
        {
            var workingDirectory = Path.Combine(this.TestContext.TestResultsDirectory, nameof(Foo2));

            var assembly = typeof(EditorConfigTests2).Assembly;


            var resources = assembly.GetManifestResourceNames();

            using (var editorConfigContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, ".editorconfig")))
            using (var projectContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Project.csproj")))
            using (var fileContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Foo3.cs")))
            {
                var solutionPath = await SetupSolution(
                    workingDirectory,
                    editorConfigContents,
                    projectContents,
                    ("Class1.cs", fileContents)).ConfigureAwait(false);
            }


            Directory.Delete(workingDirectory, true);
        }

        [TestMethod]
        public async Task Foo2()
        {
            var workingDirectory = Path.Combine(this.TestContext.TestResultsDirectory, nameof(Foo2));



            Directory.Delete(workingDirectory, true);






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

            
            ////Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
            await DoWork();
            
        }

        public static async Task DoWork()
        {
            await OpenSolution();
            var code = @"
internal class C
{
    private static int __x;

    static void M()
    {
        __x = 1; // unnecessary 'this', should trigger IDE0003
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
            var paths = new[]
            {
                @"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.dll",
            };

            ////var assembly = Assembly.Load(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll");

            return paths.SelectMany(path => Load(path)).ToImmutableArray();
        }

        public static IEnumerable<DiagnosticAnalyzer> Load(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            return assembly
                .GetTypes()
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
        }

        public static async Task OpenSolution()
        {
            //// TODO there is a superstition that this has to be called outside of the first method that uses the msbuild types; this is clearly not true, as demonstrated here
            var instance = Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();


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
                filePath: editorConfigPath).Project;

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
