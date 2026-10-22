namespace PackageTests
{
    using System;
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

            //// TODO i don't think it should be called `testhost.dll.config`

            //// TODO you should use a json config probably

            
            var value = System.Configuration.ConfigurationManager.AppSettings["MyCustomKey"]; //// TODO different config name

            Console.WriteLine(value);


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


            //// TODO make the embedded csproj files part of compilation when in the editor

        }
    }
}
