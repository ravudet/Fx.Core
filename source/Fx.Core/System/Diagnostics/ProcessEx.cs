namespace System.Diagnostics
{
#if NET5_0_OR_GREATER
    using System.Threading.Tasks;
#endif

    public static class ProcessEx //// TODO i don't think i like this naming convention, but actually maybe i do?
    {
        public static Process Start(ProcessStartInfo processStartInfo)
        {
            //// TODO exception documentation
            Process? process = null;
            try
            {
                process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();
                return process;
            }
            catch
            {
                process?.Dispose();
                throw;
            }
        }

#if NET5_0_OR_GREATER
        public static async Task<(int ExitCode, string StandardOutput, string StandardError)> Run(string command, string arguments)
        {
            using (var process = ProcessEx.Start(new ProcessStartInfo()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }))
            {
                await process.WaitForExitAsync().ConfigureAwait(false);
                var standardOutput = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var standardError = await process.StandardError.ReadToEndAsync().ConfigureAwait(false);

                return (process.ExitCode, standardOutput, standardError);
            }
        }
#endif
    }
}
