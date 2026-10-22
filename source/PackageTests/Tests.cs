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
            //// TODO i don't think it should be called `testhost.dll.config`

            //// TODO you should use a json config probably

            
            var value = System.Configuration.ConfigurationManager.AppSettings["MyCustomKey"]; //// TODO different config name

            Console.WriteLine(value);


            var workingDirectory = Path.Combine(TestContext.DeploymentDirectory, nameof(Fx_Analyzers));
            Directory.CreateDirectory(workingDirectory);
            var projectFileContents = await GetManifestResourceString("_resources.project.csproj").ConfigureAwait(false);
            File.WriteAllText(Path.Combine(workingDirectory, "project.csproj"), projectFileContents);
            var codeFileContents = await GetManifestResourceString("_resources.FX0001.cs").ConfigureAwait(false);
            File.WriteAllText(Path.Combine(workingDirectory, "FX0001.cs"), codeFileContents);

            var buildCommand = $"build \"{workingDirectory}\"";
            using (var process = Process.Start("dotnet", buildCommand))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(0, process.ExitCode);
            }

            var addPackageCommand = $"package add Fx.GlobalConfig --source \"{value}\" --project \"{workingDirectory}\" --prerelease";
            using (var process = Process.Start("dotnet", addPackageCommand))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(0, process.ExitCode);
            }

            using (var process = Process.Start("dotnet", buildCommand))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                Assert.AreEqual(1, process.ExitCode);
            }


            //// TODO make the embedded csproj files part of compilation when in the editor

            //// TODO you should bring the csproj file as a resource
            //// TODO build before and ensure %errorlevel% is 0
            //// TODO dotnet package add Fx.GlobalConfig --source c:\github\Fx.Core\packages --project c:\github\fx.core\testing --prerelease
            //// TODO build after and ensure %errorlevel% is 1 (you could also maybe assert something about the output; the output appears to be enitely standard out)
        }
    }
}
