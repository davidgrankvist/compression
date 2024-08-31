using Compression.Lib.Framework;

namespace Compression.Test.Encoders
{
    public class DummyEncoder : EncoderMiddlewareBase
    {
        public DummyEncoder(IEncoderMiddleware? next = null) : base(next)
        {

        }

        protected override byte? EncodeByte(byte input)
        {
            return input;
        }

        protected override byte? FlushByte()
        {
            return null;
        }
    }
}
