namespace Fx
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class PackageTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task FX0001()
        {
            //// TODO SOURCE_DATE_EPOCH
            //// TODO can you clean only one package and its dependencies instead of all packages from locals?
            //// TODO process.commence? instead of process.start
            //// TODO should you reconsider the assert.that pattern given that there are static extensions now?
            //// TODO does the `processex` pattern actually work? document that there are some intricacies around return value overloads; maybe `processex` works as the "alternate" name for static extensions, but that you really should just have a different pattern for return type overloads?



            //// TODO make sure you can just run the test and don't need to build the whole solution

            //// TODO once you have this corect for fx.analyzers.globalconfig.tests, add it for each other nuget package

            //// TODO make sure to have a test for everything in playground

            var diagnosticSpecifier = DiagnosticSpecifiers.FX0001;

            var packageSourceDirectory = System.Configuration.ConfigurationManager.AppSettings["PackageSourceDirectory"];
            Assert.IsNotNull(packageSourceDirectory, "PackageSourceDirectory config value is not found");

            await NugetUtility.InstallGlobalConfigPackage(
                typeof(PackageTests).Assembly,
                Path.Combine(TestContext.DeploymentDirectory, diagnosticSpecifier.TestName),
                "_resources.packagetest.csproj",
                diagnosticSpecifier.ResourceName,
                "Fx.Analyzers.GlobalConfig",
                packageSourceDirectory,
                diagnosticSpecifier.DiagnosticResult);

        }
    }
}
