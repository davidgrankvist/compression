
namespace Compression.App.Running
{
    internal class FileOrConsoleStreamProvider : IPipelineStreamProvider
    {
        public Stream CreateInputStream(string? inputFile = null)
        {
            if (inputFile == null)
            {
                return Console.OpenStandardInput();
            }
            return File.OpenRead(inputFile);
        }

        public Stream CreateOutputStream(string? outputFile = null)
        {
            if (outputFile == null)
            {
                return Console.OpenStandardOutput();
            }
            return File.OpenWrite(outputFile);
        }
    }
}
