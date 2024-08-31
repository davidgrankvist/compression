using Compression.Lib.Framework;

namespace Compression.App.Parsing
{
    public class PipelineOptions
    {
        public readonly string? InputFile;
        public readonly string? OutputFile;
        public readonly IEncoderMiddleware[] Encoders;

        public static readonly PipelineOptions Dummy = new PipelineOptions(null, null, []);

        public PipelineOptions(string? inputFile, string? outputFile, IEncoderMiddleware[] encoders)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            Encoders = encoders;
        }
    }
}
