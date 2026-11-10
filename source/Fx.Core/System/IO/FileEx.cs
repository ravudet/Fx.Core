namespace System.IO
{
    using System.Threading.Tasks;

    public static class FileEx
    {
        public static async Task WriteTextToPath(string filePath, string contents)
        {
            using (var fileStream = FileEx.OpenPathToWrite(filePath))
            {
                using (var streamWriter = new StreamWriter(fileStream, null, -1, true))
                {
                    await streamWriter.WriteAsync(contents).ConfigureAwait(false);
                }
            }
        }

        public static async Task WriteStreamToPath(string filePath, Stream contents)
        {
            using (var fileStream = FileEx.OpenPathToWrite(filePath))
            {
                await contents.CopyToAsync(fileStream).ConfigureAwait(false);
            }
        }

        public static FileStream OpenPathToWrite(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath))
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

                return fileStream;
            }
            catch
            {
                fileStream?.Dispose();
                throw;
            }
        }
    }
}
