using Compression.App.Parsing;
using Compression.Lib.Framework;

namespace Compression.App
{
    internal static class PipelineRunner
    {
        public static void Run(PipelineOptions arguments)
        {
            var pipeline = CreatePipeline(arguments.Encoders);

            using (var input = CreateInputStream(arguments.InputFile))
            using (var output = CreateOutputStream(arguments.OutputFile))
            {
                pipeline.Process(input, output);
            }
        }

        private static EncoderPipeline CreatePipeline(IEnumerable<IEncoderMiddleware> encoders)
        {
            var builder = new EncoderPipelineBuilder();
            foreach (var encoder in encoders)
            {
                builder.Add(encoder);
            }
            return builder.Build();
        }

        // TODO(testability): instead of using Console/File here, maybe pass in a stream provider via constructor?
        private static Stream CreateInputStream(string? inputFile = null)
        {
            if (inputFile == null)
            {
                return Console.OpenStandardInput();
            }
            return File.OpenRead(inputFile);
        }

        private static Stream CreateOutputStream(string? outputFile = null)
        {
            if (outputFile == null)
            {
                return Console.OpenStandardOutput();
            }
            return File.OpenWrite(outputFile);
        }
    }
}
