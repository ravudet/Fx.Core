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

            //// TODO dotnet package add {package_name_should_be_taken_from_build_properties} --source {local_path}
        }
    }
}
