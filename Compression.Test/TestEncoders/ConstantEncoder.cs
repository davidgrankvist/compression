using Compression.Lib.Framework;

namespace Compression.Test.TestEncoders
{
	internal class ConstantEncoder : EncoderMiddlewareBase
	{
		private readonly byte value;

		public byte Value => value;

        public ConstantEncoder(byte value, IEncoderMiddleware? next = null) : base(next)
		{
			this.value = value;
        }

        protected override byte? EncodeByte(byte input)
		{
			return value;
		}

		protected override byte? FlushByte()
		{
			return null;
		}
	}
}
