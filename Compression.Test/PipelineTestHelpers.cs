using Compression.Lib.Framework;

namespace Compression.Test
{
	internal static class PipelineTestHelpers
	{
		public static void CheckPipeline(EncoderPipeline pipeline, byte[] input, byte[] expectedOutput)
		{
			var outputBuffer = new byte[input.Length];

			using (var inputStream = new MemoryStream(input))
			using (var outputStream = new MemoryStream(outputBuffer, true))
			{
				pipeline.Process(inputStream, outputStream);
			}

			AssertExtensions.SequenceEqual(expectedOutput, outputBuffer);
		}
	}
}
