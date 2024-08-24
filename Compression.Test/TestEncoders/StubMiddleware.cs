using Compression.Lib.Framework;

namespace Compression.Test
{
	public class StubMiddleware : IEncoderMiddleware
	{
		public IEncoderMiddleware? Next => throw new NotImplementedException();

		public bool Encode(byte input, out byte? output)
		{
			throw new NotImplementedException();
		}

		public bool Flush(out byte? flushed)
		{
			throw new NotImplementedException();
		}
	}
}
