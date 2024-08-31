using Compression.Lib.Framework;

namespace Compression.App.Parsing
{
    internal class PipelineOptions
    {
        public readonly string? InputFile;
        public readonly string? OutputFile;
        public readonly IEnumerable<IEncoderMiddleware> Encoders;

        public PipelineOptions(string? inputFile, string? outputFile, IEnumerable<IEncoderMiddleware> encoders)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            Encoders = encoders;
        }
    }
}
