namespace PackageTests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class Tests
    {
        public TestContext TestContext { get; set; }

        private static async Task<string> GetManifestResourceString(string resourceName)
        {
            //// TODO do you want to use Assembly.GetCallingAssembly?

            return await typeof(Tests).Assembly.GetManifestResourceString(resourceName).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Fx_Analyzers()
        {


            //// TODO move these to the globalconfig test assembly
            //// TODO have one of these tests for each of the "debug" tests
            //// TODO once you have this corect for fx.analyzers.globalconfig.tests, add it for each other nuget package




            //// TODO you should use a json config probably //// https://benfoster.io/blog/net-core-configuration-legacy-projects

            var value = System.Configuration.ConfigurationManager.AppSettings["MyCustomKey"]; //// TODO different config name

            Console.WriteLine(value);


            await InstallGlobalConfigPackage(
                typeof(Tests).Assembly,
                Path.Combine(TestContext.DeploymentDirectory, nameof(Fx_Analyzers)),
                "_resources.project.csproj",
                "_resources.FX0001.cs",
                "Fx.Analyzers.GlobalConfig",
                value,
                "FX0001");

            
            


            var analyzerIdentifier = "FX0001";

            var workingDirectory = Path.Combine(TestContext.DeploymentDirectory, nameof(Fx_Analyzers));
            Directory.CreateDirectory(workingDirectory);
            var projectFileContents = await GetManifestResourceString("_resources.project.csproj").ConfigureAwait(false);
            File.WriteAllText(Path.Combine(workingDirectory, "project.csproj"), projectFileContents);
            var codeFileContents = await GetManifestResourceString($"_resources.{analyzerIdentifier}.cs").ConfigureAwait(false);
            File.WriteAllText(Path.Combine(workingDirectory, $"{analyzerIdentifier}.cs"), codeFileContents);

            var buildCommand = $"build \"{workingDirectory}\"";
            using (var process = Process.Start("dotnet", buildCommand))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(0, process.ExitCode);
            }

            var addPackageCommand = $"package add Fx.Analyzers.GlobalConfig --source \"{value}\" --project \"{workingDirectory}\" --prerelease";
            using (var process = Process.Start("dotnet", addPackageCommand))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(0, process.ExitCode);
            }

            using (var process = Process.Start("dotnet", $"restore \"{workingDirectory}\" --source \"{value}\""))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(0, process.ExitCode);
            }

            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "dotnet",
                    Arguments = buildCommand,
                    RedirectStandardOutput = true,
                },
            })
            {
                process.Start();

                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(1, process.ExitCode);

                var standardOutput = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);

                Assert.IsTrue(standardOutput.Contains(analyzerIdentifier));
            }

            //// TODO make the embedded cs files part of compilation when in the editor

        }

        public static async Task InstallGlobalConfigPackage(
            Assembly assembly,
            string workingDirectory,
            string projectResourceName,
            string codeResourceName,
            string globalConfigPackageName,
            string packageSourceDirectory,
            string analyzerIdentifier)
        {
            await assembly.WriteManifestResource(projectResourceName, Path.Combine(workingDirectory, "project.csproj")).ConfigureAwait(false);
            await assembly.WriteManifestResource(codeResourceName, Path.Combine(workingDirectory, "class.cs")).ConfigureAwait(false);

            // build the project before the `.globalconfig` package is installed to make sure that everything builds successfully (meaning that, later, when the build fails, we know it is because of the `.globalconfig` package having been installed)
            var (buildWithoutPackageExitCode, buildWithoutPackageStandardOutput, _) = await Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(0, buildWithoutPackageExitCode, buildWithoutPackageStandardOutput);

            // add the `.globalconfig` package reference to the project; we allow prerelease so that locally built packages can be tested before release
            var (packageInstallExitCode, packageInstallStandardOutput, _) = await Run("dotnet", $"package add {globalConfigPackageName} --source \"{packageSourceDirectory}\" --project \"{workingDirectory}\" --prerelease");
            Assert.AreEqual(0, packageInstallExitCode, packageInstallStandardOutput);

            // remove all packages from the local "source" so that we don't use an older, local version of the `.globalconfig` package
            var (packageCacheClearExitCode, packageCacheClearStandardOutput, _) = await Run("dotnet", $"nuget locals --clear all").ConfigureAwait(false);
            Assert.AreEqual(0, packageCacheClearExitCode, packageCacheClearStandardOutput);

            // restore the package from the package source that is populated by the local build
            var (packageRestoreExitCode, packageRestoreStandardOutput, _) = await Run("dotnet", $"restore \"{workingDirectory}\" --source \"{packageSourceDirectory}\" --no-cache --force");
            Assert.AreEqual(0, packageRestoreExitCode, packageRestoreStandardOutput);

            // build the project again now that the `.globalconfig` package has been installed and ensure that there is a build failure related to the expected analyzer identifier
            var (buildWithPackageExitCode, buildWithPackageStandardOutput, _) = await Run("dotnet", $"build \"{workingDirectory}\"").ConfigureAwait(false);
            Assert.AreEqual(1, buildWithPackageExitCode, buildWithPackageStandardOutput);
            Assert.IsTrue(buildWithPackageStandardOutput.Contains(analyzerIdentifier));
        }

        private static async Task<(int ExitCode, string StandardOutput, string StandardError)> Run(string command, string arguments)
        {
            using (var process = Process.Start(new ProcessStartInfo()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            })!) //// TODO !; read the code for `start` and make sure you can't follow a branch that returns null
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                var standardOutput = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var standardError = await process.StandardError.ReadToEndAsync().ConfigureAwait(false);

                return (process.ExitCode, standardOutput, standardError);
            }
        }
    }

    public static class Extensions
    {
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
