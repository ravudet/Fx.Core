using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fx //// TODO find the right namespace
{
    public static class NugetUtility //// TODO find the right name
    {
        public static async Task InstallGlobalConfigPackage(
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
            var (buildWithoutPackageExitCode, buildWithoutPackageStandardOutput, _) = await ProcessEx.Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(0, buildWithoutPackageExitCode, buildWithoutPackageStandardOutput);

            // add the `.globalconfig` package reference to the project; we allow prerelease so that locally built packages can be tested before release; we use no restoration so that the version numbers of the analyzer packages that the config package depends on will be ignored (this is because the version suffix of the local builds are timestamp dependent, and each project will have a different value)
            var (packageInstallExitCode, packageInstallStandardOutput, _) = await ProcessEx.Run("dotnet", $"package add {globalConfigPackageName} --source \"{packageSourceDirectory}\" --project \"{workingDirectory}\" --prerelease --no-restore");
            Assert.AreEqual(0, packageInstallExitCode, packageInstallStandardOutput);

            //// TODO removed this temporarily because you had bad internet for a bit and it caused frequent restores when building your other projects; add it back
            // remove all packages from the local "source" so that we don't use an older, local version of the `.globalconfig` package
            /*var (packageCacheClearExitCode, packageCacheClearStandardOutput, _) = await Run("dotnet", $"nuget locals --clear all").ConfigureAwait(false);
            Assert.AreEqual(0, packageCacheClearExitCode, packageCacheClearStandardOutput);*/

            // restore the package from the package source that is populated by the local build
            var (packageRestoreExitCode, packageRestoreStandardOutput, _) = await ProcessEx.Run("dotnet", $"restore \"{workingDirectory}\" --source \"{packageSourceDirectory}\" --no-cache --force");
            Assert.AreEqual(0, packageRestoreExitCode, packageRestoreStandardOutput);

            // build the project again now that the `.globalconfig` package has been installed and ensure that there is a build failure related to the expected analyzer identifier
            var (buildWithPackageExitCode, buildWithPackageStandardOutput, _) = await ProcessEx.Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(1, buildWithPackageExitCode, buildWithPackageStandardOutput);
            Assert.IsTrue(buildWithPackageStandardOutput.Contains(diagnosticResult.ToCompilerError()));
        }
    }
}
