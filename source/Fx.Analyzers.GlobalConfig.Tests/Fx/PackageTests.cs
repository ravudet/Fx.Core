namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class PackageTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task FX0001()
        {
            //// TODO make sure you can just run the test and don't need to build the whole solution

            //// TODO once you have this corect for fx.analyzers.globalconfig.tests, add it for each other nuget package

            //// TODO make sure to have a test for everything in playground

            var diagnosticSpecifier = DiagnosticSpecifiers.FX0001;

            var packageSourceDirectory = System.Configuration.ConfigurationManager.AppSettings["PackageSourceDirectory"];
            Assert.IsNotNull(packageSourceDirectory, "PackageSourceDirectory config value is not found");

            await InstallGlobalConfigPackage(
                typeof(PackageTests).Assembly,
                Path.Combine(TestContext.DeploymentDirectory, diagnosticSpecifier.TestName),
                "_resources.packagetest.csproj",
                diagnosticSpecifier.ResourceName,
                "Fx.Analyzers.GlobalConfig",
                packageSourceDirectory,
                diagnosticSpecifier.DiagnosticResult);

        }

        public static async Task InstallGlobalConfigPackage( //// TODO put this somewhere else
            Assembly assembly,
            string workingDirectory,
            string projectResourceName,
            string codeResourceName,
            string globalConfigPackageName,
            string packageSourceDirectory,
            DiagnosticResult diagnosticResult)
        {
            await assembly.WriteManifestResource(projectResourceName, Path.Combine(workingDirectory, "project.csproj")).ConfigureAwait(false);
            await assembly.WriteManifestResource(codeResourceName, Path.Combine(workingDirectory, "class.cs")).ConfigureAwait(false);

            // build the project before the `.globalconfig` package is installed to make sure that everything builds successfully (meaning that, later, when the build fails, we know it is because of the `.globalconfig` package having been installed)
            var (buildWithoutPackageExitCode, buildWithoutPackageStandardOutput, _) = await Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(0, buildWithoutPackageExitCode, buildWithoutPackageStandardOutput);

            // add the `.globalconfig` package reference to the project; we allow prerelease so that locally built packages can be tested before release; we use no restoration so that the version numbers of the analyzer packages that the config package depends on will be ignored (this is because the version suffix of the local builds are timestamp dependent, and each project will have a different value)
            var (packageInstallExitCode, packageInstallStandardOutput, _) = await Run("dotnet", $"package add {globalConfigPackageName} --source \"{packageSourceDirectory}\" --project \"{workingDirectory}\" --prerelease --no-restore");
            Assert.AreEqual(0, packageInstallExitCode, packageInstallStandardOutput);

            //// TODO removed this temporarily because you had bad internet for a bit and it caused frequent restores when building your other projects; add it back
            // remove all packages from the local "source" so that we don't use an older, local version of the `.globalconfig` package
            /*var (packageCacheClearExitCode, packageCacheClearStandardOutput, _) = await Run("dotnet", $"nuget locals --clear all").ConfigureAwait(false);
            Assert.AreEqual(0, packageCacheClearExitCode, packageCacheClearStandardOutput);*/

            // restore the package from the package source that is populated by the local build
            var (packageRestoreExitCode, packageRestoreStandardOutput, _) = await Run("dotnet", $"restore \"{workingDirectory}\" --source \"{packageSourceDirectory}\" --no-cache --force");
            Assert.AreEqual(0, packageRestoreExitCode, packageRestoreStandardOutput);

            // build the project again now that the `.globalconfig` package has been installed and ensure that there is a build failure related to the expected analyzer identifier
            var (buildWithPackageExitCode, buildWithPackageStandardOutput, _) = await Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(1, buildWithPackageExitCode, buildWithPackageStandardOutput);
            Assert.IsTrue(buildWithPackageStandardOutput.Contains(diagnosticResult.ToCompilerError()));
        }

        private static async Task<(int ExitCode, string StandardOutput, string StandardError)> Run(string command, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                process.Start(); //// TODO you needed to do this because `Process.Start()` return always returns true, but process.start(info) returns a nullable; how do you want to share this knowledge? you can't re-use the class name `process`

                await process.WaitForExitAsync().ConfigureAwait(false);
                var standardOutput = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var standardError = await process.StandardError.ReadToEndAsync().ConfigureAwait(false);

                return (process.ExitCode, standardOutput, standardError);
            }
        }
    }

    public static class Extensions //// TODO put these somewhere else
    {
        public static string ToCompilerError(this DiagnosticResult diagnosticResult)
        {
            var compilerError = new StringBuilder();
            if (diagnosticResult.Spans.TryFirst(out var location))
            {
                compilerError.Append($"({location.Span.StartLinePosition.Line + 1},{location.Span.StartLinePosition.Character + 1}): ");
            }

            compilerError.Append($"{diagnosticResult.Severity.ToString().ToLowerInvariant()} {diagnosticResult.Id}:");

            return compilerError.ToString();
        }

        public static bool TryFirst<TElement>(this IEnumerable<TElement> source, [MaybeNullWhen(false)] out TElement first)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    first = default(TElement);
                    return false;
                }

                first = enumerator.Current;
                return true;
            }
        }

        public static async Task WriteManifestResource(this Assembly assembly, string resourceName, string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new Exception("TODO");
            }

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception("TODO");
                }

                FileStream? fileStream = null;
                try
                {
                    while (true)
                    {
                        try
                        {
                            fileStream = File.OpenWrite(filePath);
                            break;
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                    }

                    await resourceStream.CopyToAsync(fileStream).ConfigureAwait(false);
                }
                finally
                {
                    fileStream?.Dispose();
                }
            }
        }
    }
}
