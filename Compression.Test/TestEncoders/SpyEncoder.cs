using Compression.Lib.Framework;

namespace Compression.Test.TestEncoders
{
    internal class SpyEncoder : EncoderMiddlewareBase
    {
        private readonly byte[] spyBuffer;
        private int bufferIndex;

        public byte[] Buffer => spyBuffer;

        public SpyEncoder(int bufferSize, IEncoderMiddleware? next = null) : base(next)
        {
            spyBuffer = new byte[bufferSize];
            bufferIndex = 0;
        }

        protected override byte? EncodeByte(byte input)
        {
            spyBuffer[bufferIndex] = input;
            bufferIndex = (bufferIndex + 1) % spyBuffer.Length;

            return input;
        }

        protected override byte? FlushByte()
        {
            return null;
        }
    }
}
