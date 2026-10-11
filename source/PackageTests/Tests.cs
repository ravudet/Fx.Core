namespace PackageTests
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class Tests
    {
        [TestMethod]
        public void Fx_Analyzers()
        {
            //// TODO i don't think it should be called `testhost.dll.config`

            //// TODO you should use a json config probably

            
            var value = System.Configuration.ConfigurationManager.AppSettings["MyCustomKey"];

            Console.WriteLine(value);




            //// TODO you should bring the csproj file as a resource
            //// TODO build before and ensure %errorlevel% is 0
            //// TODO dotnet package add Fx.GlobalConfig --source c:\github\Fx.Core\packages --project c:\github\fx.core\testing --prerelease
            //// TODO build after and ensure %errorlevel% is 1 (you could also maybe assert something about the output; the output appears to be enitely standard out)
        }
    }
}
