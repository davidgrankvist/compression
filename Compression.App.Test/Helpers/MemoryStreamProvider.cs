using Compression.App.Running;

namespace Compression.App.Test.Helpers
{
    internal class MemoryStreamProvider : IPipelineStreamProvider
    {
        public string? InputFile { get; private set; }
        public string? OutputFile { get; private set; }

        private readonly byte[] input;
        private readonly byte[] output;

        public MemoryStreamProvider(byte[] input, byte[] output)
        {
            this.input = input;
            this.output = output;
        }

        public Stream CreateInputStream(string? inputFile = null)
        {
            InputFile = inputFile;
            return new MemoryStream(input);
        }

        public Stream CreateOutputStream(string? outputFile = null)
        {
            OutputFile = outputFile;
            return new MemoryStream(output, true);
        }
    }
}
