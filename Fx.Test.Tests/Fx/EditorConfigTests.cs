// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var result = Path.Combine(paths).Replace(Path.DirectorySeparatorChar, '.');
            return result;
        }

        private const string EmbeddedResourceRootPath = "Content";

        [TestMethod]
        public async Task Foo3()
        {
            //// TODO even though you've enabled ide-only triggering on build, some of them (like file header) aren't triggering; they *do* trigger on `dotnet build` though, which makes me think that you're just not loading that analyzer somehow //// TODO and you confirmed that a change to 1052 in the `.editorconfig` was reflected in the compiler diagnostics
            
            //// TODO current method name
            var workingDirectory = Path.Combine(this.TestContext.DeploymentDirectory, nameof(Foo3));

            string solutionPath;

            var assembly = typeof(EditorConfigTests2).Assembly;
            using (var editorConfigContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, ".editorconfig")))
            using (var projectContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Project.csproj")))
            using (var fileContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Foo3.cs")))
            {
                solutionPath = await SetupSolution(
                    workingDirectory,
                    editorConfigContents,
                    projectContents,
                    ("Class1.cs", fileContents)).ConfigureAwait(false);
            }

            //// TODO productize `loadall`; in fact, you generally hate this sort of thing and prefer precision, like knowing the exact analyzer this test will use
            var diagnostics = CompileSolution(solutionPath, LoadAll()).SelectMany(project => project.Diagnostics);

            var diagnostic = await diagnostics.Where(diagnostic => diagnostic.Id == "CA1052").First().ConfigureAwait(false);

            Assert.IsTrue(diagnostic.Location.SourceTree.FilePath.EndsWith("Class1.cs"));
            Assert.AreEqual(45, diagnostic.Location.SourceSpan.Start);
            Assert.AreEqual(51, diagnostic.Location.SourceSpan.End);

            Directory.Delete(workingDirectory, true);
        }

        [TestMethod]
        public async Task Foo4()
        {
            //// TODO current method name
            var workingDirectory = Path.Combine(this.TestContext.DeploymentDirectory, nameof(Foo4));

            string solutionPath;

            var assembly = typeof(EditorConfigTests2).Assembly;

            var paths = assembly.GetManifestResourceNames();

            using (var editorConfigContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, ".editorconfig")))
            using (var projectContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Project.csproj")))
            using (var analyzer1AnalzyerContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Analyzer1Analyzer.cs")))
            using (var resourcesResxContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Resources.resources")))
            using (var resourcesDesignerContents = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Resources.Designer.cs")))
            {
                solutionPath = await SetupSolution(
                    workingDirectory,
                    editorConfigContents,
                    projectContents,
                    ("Analyzer1Analzyer.cs", analyzer1AnalzyerContents),
                    ("Resources.resx", resourcesResxContents),
                    ("Resources.Designer.cs", resourcesDesignerContents)).ConfigureAwait(false);
            }

            //// TODO productize `loadall`; in fact, you generally hate this sort of thing and prefer precision, like knowing the exact analyzer this test will use
            var diagnostics = CompileSolution(solutionPath, LoadAll()).SelectMany(project => project.Diagnostics);

            var diagnostic = await diagnostics.Where(diagnostic => diagnostic.Id == "CA1052").First().ConfigureAwait(false);

            Assert.IsTrue(diagnostic.Location.SourceTree.FilePath.EndsWith("Class1.cs"));
            Assert.AreEqual(45, diagnostic.Location.SourceSpan.Start);
            Assert.AreEqual(51, diagnostic.Location.SourceSpan.End);

            Directory.Delete(workingDirectory, true);
        }

        public static async IAsyncEnumerable<(string ProjectId, ImmutableArray<Diagnostic> Diagnostics)> CompileSolution(string solutionPath, ImmutableArray<DiagnosticAnalyzer> analyzers)
        {
            //// TODO there is a superstition that this has to be called outside of the first method that uses the msbuild types; this is clearly not true, as demonstrated here
            var instance = Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();


            ////

            ////var solutionPath = @"C:\github\OddTrotter\Fx.Core\Fx.Core.sln";
            //// TODO package `Microsoft.CodeAnalysis.Workspaces.MSBuild` actually depends on `Microsoft.Build`
            var workspace = MSBuildWorkspace.Create();

            /*var loader = new MSBuildProjectLoader(workspace);
            var solutionInfo = await loader.LoadSolutionInfoAsync(solutionPath);


            var projectInfo = await loader.LoadProjectInfoAsync(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Fx.Test.Tests.csproj");*/

            var solution = await workspace.OpenSolutionAsync(solutionPath);

            foreach (var project in solution.Projects)
            {
                var reference = CompilationReference.CreateFromFile()

                var references = project.MetadataReferences;

                var compilation = await project.GetCompilationAsync();
                var withAnalyzers = compilation.WithAnalyzers(analyzers);
                //var diagnostics = compilation.GetDiagnostics();
                var diagnostics = await withAnalyzers.GetAllDiagnosticsAsync();

                yield return (project.Id.Id.ToString(), diagnostics);
            }


            /*var project = solution.Projects.Where(project => project.Name == "ClassLibrary1").First();

            solution = solution.WithProjectCompilationOptions(project.Id, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            project = solution.Projects.Where(project => project.Name == "ClassLibrary1").First();

            var compilation = await project.GetCompilationAsync();
            var withAnalyzers = compilation.WithAnalyzers(LoadAll());
            //var diagnostics = compilation.GetDiagnostics();
            var diagnostics = await withAnalyzers.GetAllDiagnosticsAsync();*/
        }


        public static ImmutableArray<DiagnosticAnalyzer> LoadAll()
        {
            var paths = new[]
            {
                typeof(Microsoft.NetFramework.Analyzers.TypesShouldNotExtendCertainBaseTypesAnalyzer).Assembly.Location,
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll",
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

            var analyzers = assembly
                .GetTypes()
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract);

            var publicAnalyzers = analyzers.Where(analyzer => analyzer.IsPublic);

            return analyzers.Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
        }
    }

    public static class AsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            await foreach (var element in source.ConfigureAwait(false))
            {
                foreach (var selectedElement in selector(element))
                {
                    yield return selectedElement;
                }
            }
        }

        public static async IAsyncEnumerable<TElement> Where<TElement>(
            this IAsyncEnumerable<TElement> source,
            Func<TElement, bool> predicate)
        {
            await foreach (var element in source.ConfigureAwait(false))
            {
                if (predicate(element))
                {
                    yield return element;
                }
            }
        }

        public static async Task<TElement> First<TElement>(
            this IAsyncEnumerable<TElement> source)
        {
            IAsyncEnumerator<TElement>? enumerator = null; //// TODO figure out a way to not need all of this boilerplate
            try
            {
                enumerator = source.GetAsyncEnumerator();
                if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException("TODO");
                }

                return enumerator.Current;
            }
            finally
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
