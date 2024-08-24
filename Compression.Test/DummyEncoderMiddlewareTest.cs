using Compression.Test.Encoders;

namespace Compression.Test
{
	[TestClass]
	public class DummyEncoderMiddlewareTest
	{
		[TestMethod]
		public void ShouldEncodeByteAsSameData()
		{
			var dummyEncoder = new DummyEncoder();
			byte input = 123;

			var didEncode = dummyEncoder.Encode(input, out var output);

			Assert.IsTrue(didEncode);
			Assert.AreEqual(input, output);
		}

		[TestMethod]
		public void ShouldNotFlush()
		{
			var dummyEncoder = new DummyEncoder();

			var didFlush = dummyEncoder.Flush(out var output);

			Assert.IsFalse(didFlush);
			Assert.IsNull(output);
		}
	}
}