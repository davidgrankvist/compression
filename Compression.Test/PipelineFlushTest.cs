using Compression.Lib.Framework;
using Compression.Test.Helpers;
using Compression.Test.TestEncoders;

namespace Compression.Test
{
    [TestClass]
	public class PipelineFlushTest
	{
		[TestMethod]
		public void ShouldOutputFlushedValues()
		{
			byte target = 0;
			byte other = 1;
			var input = new byte[] { target, target, target, target, other, target, other };
			var expected = input.OrderDescending().ToArray();

			var dummyBufferEncoder = new DummyBufferEncoder(target);
			var pipeline = new EncoderPipeline(dummyBufferEncoder);

			PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
		}

		[TestMethod]
		public void ShouldDelegateFlushedValues()
		{
			byte target = 0;
			byte other = 1;
			byte final = 2;
			var input = Enumerable.Repeat(target, 5).ToArray();
			var expected = Enumerable.Repeat(final, 5).ToArray();

			var pipeline = new EncoderPipelineBuilder()
				.Add(new DummyBufferEncoder(target))
				.Add(new ConstantEncoder(other))
				.Add(new DummyBufferEncoder(other))
				.Add(new ConstantEncoder(final))
				.Build();

			PipelineTestHelpers.CheckPipeline(pipeline, input, expected);
		}

		[TestMethod]
		public void ShouldDelegateMixedFlushedValues()
		{
			byte target = 0;
			byte other = 1;
			var input = new byte[] { target, other, target, other };

			var firstSpy = new SpyEncoder(input.Length);
			var secondSpy = new SpyEncoder(input.Length);
			var thirdSpy = new SpyEncoder(input.Length);

			var pipeline = new EncoderPipelineBuilder()
				.Add(firstSpy)
				.Add(new DummyBufferEncoder(target))
				.Add(secondSpy)
				.Add(new DummyBufferEncoder(other))
				.Add(thirdSpy)
				.Build();
			PipelineTestHelpers.RunPipeline(pipeline, input);

			// encoding calls before flush
			AssertExtensions.SequenceEqual(firstSpy.Buffer, input);
			// 2x other passed through before flush, 2x target are flushed
			AssertExtensions.SequenceEqual(secondSpy.Buffer, [other, other, target, target]);
			// 2x target are flushed, 2x other are flushed
			AssertExtensions.SequenceEqual(thirdSpy.Buffer, [target, target, other, other]);
		}
	}
}
