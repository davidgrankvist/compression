using Compression.Lib.Framework;

namespace Compression.Test.TestEncoders
{
	internal class DummyOutputBufferEncoder : EncoderMiddlewareBase
	{
		private Queue<byte> outputBuffer;

        public DummyOutputBufferEncoder(IEncoderMiddleware? next = null) : base(next)
        {
            outputBuffer = new Queue<byte>();
        }

        protected override byte? EncodeByte(byte input)
		{
			var firstOutput = input;
			var secondOutput = input;
			outputBuffer.Enqueue(secondOutput);

			return firstOutput;
		}

		protected override byte? FlushByte()
		{
			return FlushOutputByte();
		}

		protected override byte? FlushOutputByte()
		{
			if (outputBuffer.TryDequeue(out var output))
			{
				return output;
			}

			return null;
		}

		public override bool HasPendingOutput()
		{
			return outputBuffer.Count > 0;
		}
	}
}
