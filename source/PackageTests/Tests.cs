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
            //// TODO you should use a json config probably

            Console.WriteLine(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);

            var value = System.Configuration.ConfigurationManager.AppSettings["MyCustomKey"];

            Console.WriteLine(value);

            var value2 = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

            Console.WriteLine(value2);

            var value3 = System.Configuration.ConfigurationManager.AppSettings["BuildNumber"];

            Console.WriteLine(value3);

            //// TODO dotnet package add {package_name_should_be_taken_from_build_properties} --source {local_path}
        }
    }
}
