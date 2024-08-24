using Compression.Lib.Framework;

namespace Compression.Test
{
	internal static class PipelineTestHelpers
	{
		public static void CheckPipeline(EncoderPipeline pipeline, byte[] input, byte[] expectedOutput)
		{
			var output = RunPipeline(pipeline, input);

			AssertExtensions.SequenceEqual(expectedOutput, output);
		}

		public static byte[] RunPipeline(EncoderPipeline pipeline, byte[] input)
		{
			var outputBuffer = new byte[input.Length];

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
