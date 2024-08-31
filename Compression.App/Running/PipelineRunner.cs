using Compression.App.Parsing;
using Compression.Lib.Framework;

namespace Compression.App.Running
{
    public static class PipelineRunner
    {
        public static void Run(PipelineOptions arguments, IPipelineStreamProvider streamProvider)
        {
            var pipeline = CreatePipeline(arguments.Encoders);

            using (var input = streamProvider.CreateInputStream(arguments.InputFile))
            using (var output = streamProvider.CreateOutputStream(arguments.OutputFile))
            {
                pipeline.Process(input, output);
            }
        }

        private static EncoderPipeline CreatePipeline(IEncoderMiddleware[] encoders)
        {
            var builder = new EncoderPipelineBuilder();
            foreach (var encoder in encoders)
            {
                builder.Add(encoder);
            }
            return builder.Build();
        }
    }
}
