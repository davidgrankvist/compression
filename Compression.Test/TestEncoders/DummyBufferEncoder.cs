using Compression.Lib.Framework;

namespace Compression.Test.TestEncoders
{
    internal class DummyBufferEncoder : EncoderMiddlewareBase
    {
        private readonly byte target;
        private int numBuffered;

        public DummyBufferEncoder(byte target, IEncoderMiddleware? next = null) : base(next)
        {
            this.target = target;
            numBuffered = 0;
        }

        protected override byte? EncodeByte(byte input)
        {
            if (input != target)
            {
                return input;
            }

            numBuffered++;
            return null;
        }

        protected override byte? FlushByte()
        {
            if (numBuffered > 0)
            {
                numBuffered--;
                return target;
            }

            return null;
        }
    }
}
