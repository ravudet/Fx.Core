namespace System.Reflection
{
    using System.IO;
    using System.Threading.Tasks;

    public static class AssemblyExtensions
    {
        public static async Task<string> GetManifestResourceString(this Assembly assembly, string resourceName)
        {
            //// TODO have a sync overload?

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception("TODO");
                }

                //// TODO parameterize the constructor parameters
                using (var streamReader = new StreamReader(resourceStream))
                {
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public static async Task WriteManifestResource(this Assembly assembly, string resourceName, string filePath)
        {
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception("TODO");
                }

                await FileEx.WriteStreamToPath(filePath, resourceStream).ConfigureAwait(false);
            }
        }
    }
}
