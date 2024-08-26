using Compression.Lib.Framework;

namespace Compression.Test.Helpers
{
    internal static class PipelineTestHelpers
    {
        public static void CheckPipeline(EncoderPipeline pipeline, byte[] input, byte[] expectedOutput)
        {
            var output = RunPipeline(pipeline, input, expectedOutput.Length);

            AssertExtensions.SequenceEqual(expectedOutput, output);
        }

        public static byte[] RunPipeline(EncoderPipeline pipeline, byte[] input, int? outputSize = null)
        {
            var outputBuffer = new byte[outputSize ?? input.Length];

            using (var inputStream = new MemoryStream(input))
            using (var outputStream = new MemoryStream(outputBuffer, true))
            {
                pipeline.Process(inputStream, outputStream);
            }

            return outputBuffer;
        }

        public static IEnumerable<IEncoderMiddleware> GetMiddleWares(IEncoderMiddleware middleware)
        {
            if (middleware.Next == null)
            {
                return [middleware];
            }

            var middlewares = new List<IEncoderMiddleware>();
            var current = middleware;
            while (current != null)
            {
                middlewares.Add(current);
                current = current.Next;
            }

            return middlewares;
        }
    }
}
