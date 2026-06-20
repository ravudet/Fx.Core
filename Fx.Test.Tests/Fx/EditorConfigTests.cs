// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.NetCore.Analyzers.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class FileUtilities //// TODO productize these
    {
        public static async Task WriteAllContents(string filePath, string contents)
        {
            using (var file = CreateFileWrite(filePath))
            {
                using (var textWriter = new StreamWriter(file))
                {
                    await textWriter.WriteAsync(contents).ConfigureAwait(false);
                }
            }
        }

        public static async Task WriteAllContents(string filePath, Stream contents)
        {
            using (var file = CreateFileWrite(filePath))
            {
                await contents.CopyToAsync(file).ConfigureAwait(false);
            }
        }

        public static Stream CreateFileWrite(string filePath)
        {
            return OpenFileWrite(filePath, FileMode.CreateNew);
        }

        public static Stream OpenFileWrite(string filePath, FileMode fileMode)
        {
            return OpenFile(filePath, fileMode, FileAccess.Write, FileShare.None);
        }

        public static Stream OpenFile(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            //// TODO this isn't "openfile"; you shouldn't create a directory for reaed operations, for example

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
    }

    public class SolutionSpecification
    {
        public SolutionSpecification(IEnumerable<FileSpecification> files, IEnumerable<ProjectSpecification> projects)
        {
            this.Files = files;
            this.Projects = projects;
        }

        public IEnumerable<FileSpecification> Files { get; }

        public IEnumerable<ProjectSpecification> Projects { get; }
    }

    public class ProjectSpecification
    {
        public ProjectSpecification(string name, Stream contents, IEnumerable<FileSpecification> files)
        {
            this.Name = name;
            this.Contents = contents;
            this.Files = files;
        }

        public string Name { get; }

        public Stream Contents { get; }

        public IEnumerable<FileSpecification> Files { get; }
    }

    public class FileSpecification
    {
        public FileSpecification(string pathRelativeToContainerRoot, Stream contents)
        {
            this.PathRelativeToContainerRoot = pathRelativeToContainerRoot;
            this.Contents = contents;
        }

        public string PathRelativeToContainerRoot { get; }

        public Stream Contents { get; }
    }

    public static class SolutionUtilities
    {
        private static void CreateIds(string path, Dictionary<string, (Guid Id, HashSet<string> FileNames)> folderToIdMapping)
        {
            var parent = Path.GetDirectoryName(path);
            while (!string.IsNullOrEmpty(parent))
            {
                folderToIdMapping.TryAdd(parent, (Guid.NewGuid(), new HashSet<string>()));
                parent = Path.GetDirectoryName(parent);
            }
        }

        public static async Task<string> SetupSolution(string solutionDirectory, SolutionSpecification solutionSpecification)
        {
            //// TODO you haven't tested anything with solution files that aren't in the "solution items" folder

            var projectPaths = new List<(string ProjectName, string ProjectPath)>();
            foreach (var project in solutionSpecification.Projects) //// TODO move this inside the using statement below
            {
                var projectPath = await SolutionUtilities
                    .SetupProject(
                        Path.Combine(solutionDirectory, project.Name),
                        project)
                    .ConfigureAwait(false);
                projectPaths.Add((project.Name, projectPath));
            }

            var solutionPath = Path.Combine(solutionDirectory, "solution.sln");
            using (var solutionFile = FileUtilities.CreateFileWrite(solutionPath))
            {
                using (var textWriter = new StreamWriter(solutionFile))
                {
                    await textWriter
                        .WriteLineAsync(
$$"""
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.7.34031.279
MinimumVisualStudioVersion = 10.0.40219.1
"""
                        ).ConfigureAwait(false);

                    var projectGuids = new List<Guid>();
                    foreach (var projectPath in projectPaths)
                    {
                        var projectGuid = Guid.NewGuid();
                        projectGuids.Add(projectGuid);
                        var projectRelativePath = Path.GetRelativePath(solutionDirectory, projectPath.ProjectPath);
                        await textWriter
                            .WriteLineAsync(
$$"""
Project("{{{Guid.NewGuid()}}}") = "{{projectPath.ProjectName}}", "{{projectRelativePath}}", "{{{projectGuid}}}"
EndProject
"""
                            ).ConfigureAwait(false);
                    }

                    var solutionFolderToIdMapping = new Dictionary<string, (Guid Id, HashSet<string> FileNames)>()
                    {
                        { string.Empty, (Guid.Parse("02EA681E-C7D8-13C7-8484-4AC65E1B71E8"), new HashSet<string>()) },
                    };
                    foreach (var fileSpecification in solutionSpecification.Files)
                    {
                        CreateIds(fileSpecification.PathRelativeToContainerRoot, solutionFolderToIdMapping);
                        var parent = Path.GetDirectoryName(fileSpecification.PathRelativeToContainerRoot);
                        solutionFolderToIdMapping.TryGetValue(parent, out var id);

                        var fileName = Path.GetFileName(fileSpecification.PathRelativeToContainerRoot);
                        if (!id.FileNames.Add(fileName))
                        {
                            throw new Exception("TODO duplicate solution file");
                        }

                        await FileUtilities.WriteAllContents(Path.Combine(solutionDirectory, fileSpecification.PathRelativeToContainerRoot), fileSpecification.Contents).ConfigureAwait(false);
                    }

                    foreach (var solutionFolder in solutionFolderToIdMapping)
                    {
                        Guid id;
                        string folderName;
                        if (string.IsNullOrEmpty(solutionFolder.Key))
                        {
                            id = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8");
                            folderName = "Solution Items";
                        }
                        else
                        {
                            id = Guid.NewGuid();
                            folderName = Path.GetFileName(solutionFolder.Key);
                        }

                        await textWriter.WriteLineAsync(
$$"""
Project("{{{id}}}") = "{{folderName}}", "{{folderName}}", "{{{solutionFolder.Value.Id}}}"
	ProjectSection(SolutionItems) = preProject
"""
                        ).ConfigureAwait(false);

                        foreach (var fileName in solutionFolder.Value.FileNames)
                        {
                            await textWriter.WriteLineAsync(
$$"""
		{{fileName}} = {{fileName}}
"""
                            ).ConfigureAwait(false);
                        }

                        await textWriter.WriteLineAsync(
$$"""
	EndProjectSection
EndProject
"""
                        ).ConfigureAwait(false);
                    }

                    await textWriter.WriteLineAsync(
$$"""
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
"""
                        ).ConfigureAwait(false);

                    foreach (var projectGuid in projectGuids)
                    {
                        await textWriter.WriteLineAsync(
$$"""
		{{{projectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{{projectGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{{projectGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{{projectGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU
"""
                            ).ConfigureAwait(false);
                    }

                    await textWriter.WriteLineAsync(
$$"""
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
"""
                    ).ConfigureAwait(false);

                    foreach (var solutionFolder in solutionFolderToIdMapping)
                    {
                        if (string.IsNullOrEmpty(solutionFolder.Key))
                        {
                            continue;
                        }

                        var parentPath = Path.GetDirectoryName(solutionFolder.Key);
                        if (string.IsNullOrEmpty(parentPath))
                        {
                            continue;
                        }

                        var parentId = solutionFolderToIdMapping[parentPath];

                        await textWriter.WriteLineAsync(
$$"""
		{{{solutionFolder.Value.Id}}} = {{{parentId}}}
"""
                        ).ConfigureAwait(false);
                    }

                    await textWriter.WriteLineAsync(
$$"""
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {938EAC26-C20C-48C0-B5F6-0B535D593D6B}
	EndGlobalSection
EndGlobal
"""
                    ).ConfigureAwait(false);
                }
            }

            //// TODO create solution instance //// TODO do y ou actually want to do this, or should the project embedded resouirce just reference relative paths to assemblies?
            return solutionPath;
        }

        public static async Task<string> SetupProject(
            string projectDirectory,
            ProjectSpecification projectSpecification)
        {
            var projectPath = Path.Combine(projectDirectory, $"{projectSpecification.Name}.csproj");
            await FileUtilities.WriteAllContents(projectPath, projectSpecification.Contents).ConfigureAwait(false);
            foreach (var fileSpecification in projectSpecification.Files)
            {
                var filePath = Path.Combine(projectDirectory, fileSpecification.PathRelativeToContainerRoot);
                await FileUtilities.WriteAllContents(filePath, fileSpecification.Contents).ConfigureAwait(false);
            }

            return projectPath;
        }
    }

    

    [TestClass]
    public class EditorConfigTests2
    {
        [TestMethod]
        public void DirectoryTest()
        {
            var directory = Path.GetDirectoryName("asdf");
        }

        private static async Task<string> SetupSolution(
            string workingDirectory,
            Stream editorConfigContents,
            Stream projectContents,
            params (string FilePathRelativeToProjectRoot, Stream FileContents)[] files)
        {
            // returns the path to the solution file

            await WriteAllContents(
                Path.Combine(workingDirectory, ".editorconfig"),
                "root = true").ConfigureAwait(false); // adding this so that the editorconfig that is being tested doesn't accidentally interact with something in the working directory (or above);

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

        private static async Task WriteAllContents(string filePath, string contents)
        {
            using (var file = CreateFileWrite(filePath))
            {
                using (var textWriter = new StreamWriter(file))
                {
                    await textWriter.WriteAsync(contents).ConfigureAwait(false);
                }
            }
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
            //// TODO this isn't "openfile"; you shouldn't create a directory for reaed operations, for example

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
                var solutionSpecification = new SolutionSpecification(
                    new[]
                    {
                        new FileSpecification(
                            ".editorConfig",
                            editorConfigContents),
                    },
                    new[]
                    {
                        new ProjectSpecification(
                            "Project",
                            projectContents,
                            new[]
                            {
                                new FileSpecification(
                                    "Foo3.cs",
                                    fileContents),
                            })
                    });

                solutionPath = await SolutionUtilities
                    .SetupSolution(
                        Path.Combine(workingDirectory, "solution"),
                        solutionSpecification)
                    .ConfigureAwait(false);
            }

            //// TODO productize `loadall`; in fact, you generally hate this sort of thing and prefer precision, like knowing the exact analyzer this test will use
            var diagnostics = CompileSolution(solutionPath, LoadAll()).SelectMany(project => project.Diagnostics);

            var diagnostic = await diagnostics.Where(diagnostic => diagnostic.Id == "CA1052").First().ConfigureAwait(false);

            Assert.IsTrue(diagnostic.Location.SourceTree.FilePath.EndsWith("Foo3.cs"));
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
            using (var microsoftCodeAnalysisAnalyzersContent = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Microsoft.CodeAnalysis.Analyzers.dll")))
            using (var microsoftCodeAnalysisCsharpAnalyzersContent = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Microsoft.CodeAnalysis.CSharp.Analyzers.dll")))
            using (var microsoftCodeAnalysisCsharpContent = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Microsoft.CodeAnalysis.CSharp.dll")))
            using (var microsoftCodeAnalysisContent = assembly.GetManifestResourceStream(CombineResourcePath(EmbeddedResourceRootPath, "Analyzer", "Microsoft.CodeAnalysis.dll")))
            {
                //// TODO you are here
                //// TODO get an actual analyzer test working (you will need to load the correct analyzers to pass to `compilesolution`

                var solutionSpecification = new SolutionSpecification(
                    new[]
                    {
                        new FileSpecification(
                            ".editorConfig",
                            editorConfigContents),
                    },
                    new[]
                    {
                        new ProjectSpecification(
                            "Project",
                            projectContents,
                            new[]
                            {
                                new FileSpecification(
                                    "Analyzer1Analyzer.cs",
                                    analyzer1AnalzyerContents),
                                new FileSpecification(
                                    "Resources.resx",
                                    resourcesResxContents),
                                new FileSpecification(
                                    "Resources.Designer.cs",
                                    resourcesDesignerContents),
                                new FileSpecification(
                                    "Microsoft.CodeAnalysis.Analyzers.dll",
                                    microsoftCodeAnalysisAnalyzersContent),
                                new FileSpecification(
                                    "Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
                                    microsoftCodeAnalysisCsharpAnalyzersContent),
                                new FileSpecification(
                                    "Microsoft.CodeAnalysis.CSharp.dll",
                                    microsoftCodeAnalysisCsharpContent),
                                new FileSpecification(
                                    "Microsoft.CodeAnalysis.dll",
                                    microsoftCodeAnalysisContent),
                            })
                    });

                solutionPath = await SolutionUtilities
                    .SetupSolution(
                        Path.Combine(workingDirectory, "solution"),
                        solutionSpecification)
                    .ConfigureAwait(false);

                /*solutionPath = await SetupSolution(
                    workingDirectory,
                    editorConfigContents,
                    projectContents,
                    ("Analyzer1Analzyer.cs", analyzer1AnalzyerContents),
                    ("Resources.resx", resourcesResxContents),
                    ("Resources.Designer.cs", resourcesDesignerContents)).ConfigureAwait(false);*/
            }

            //// TODO productize `loadall`; in fact, you generally hate this sort of thing and prefer precision, like knowing the exact analyzer this test will use
            var diagnostics = CompileSolution(solutionPath, LoadAll()).SelectMany(project => project.Diagnostics);

            var diagnostic = await diagnostics.Where(diagnostic => diagnostic.Id == "RS1001").First().ConfigureAwait(false);

            Assert.IsTrue(diagnostic.Location.SourceTree.FilePath.EndsWith("Analyzer1Analyzer.cs"));
            Assert.AreEqual(568, diagnostic.Location.SourceSpan.Start);
            Assert.AreEqual(585, diagnostic.Location.SourceSpan.End);

            Directory.Delete(workingDirectory, true);
        }

        private static int DefaultsRegistered = 0;

        public async IAsyncEnumerable<(string ProjectId, ImmutableArray<Diagnostic> Diagnostics)> CompileSolution(string solutionPath, ImmutableArray<DiagnosticAnalyzer> analyzers) //// TODO make this static
        {

            if (Interlocked.Exchange(ref DefaultsRegistered, 1) == 0)
            {
                //// TODO there is a superstition that this has to be called outside of the first method that uses the msbuild types; this is clearly not true, as demonstrated here
                var instance = Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
            }

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
                /*var reference1 = CompilationReference.CreateFromFile(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\stuff\Microsoft.CodeAnalysis.Analyzers.dll");
                var reference2 = CompilationReference.CreateFromFile(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\stuff\Microsoft.CodeAnalysis.CSharp.Analyzers.dll");
                var reference3 = CompilationReference.CreateFromFile(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\stuff\Microsoft.CodeAnalysis.CSharp.dll");
                var reference4 = CompilationReference.CreateFromFile(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\stuff\Microsoft.CodeAnalysis.dll");*/

                var project2 = project
                    /*.AddMetadataReference(reference1)
                    .AddMetadataReference(reference2)
                    .AddMetadataReference(reference3)
                    .AddMetadataReference(reference4)*/
                    ;

                /*var path = Path.Combine(this.TestContext.DeploymentDirectory, nameof(Foo4), "solution", "Project", "Resources.resx");
                project2 = project2
                    .AddAdditionalDocument(
                        "Resources.resx",
                        await File.ReadAllTextAsync(path).ConfigureAwait(false),
                        null,
                        path)
                    .Project;*/

                //// TODO there are too many `analyzerconfigdocuments`

                var compilation = await project2.GetCompilationAsync();
                var withAnalyzers = compilation.WithAnalyzers(analyzers);
                //var diagnostics = compilation.GetDiagnostics();
                var diagnostics = await withAnalyzers.GetAllDiagnosticsAsync();

                Assert.IsFalse(diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error && !diagnostic.IsWarningAsError).Any());

                yield return (project2.Id.Id.ToString(), diagnostics);
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

            var type = typeof(Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.DiagnosticAnalyzerAttributeAnalyzer);
            var another = (DiagnosticAnalyzer)Activator.CreateInstance(type)!;

            return paths.SelectMany(path => Load(path)).Append(another).ToImmutableArray();
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
