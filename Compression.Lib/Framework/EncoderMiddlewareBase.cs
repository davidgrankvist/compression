namespace Compression.Lib.Framework
{
    public abstract class EncoderMiddlewareBase : IEncoderMiddleware
    {
        private readonly IEncoderMiddleware? next;

        protected EncoderMiddlewareBase(IEncoderMiddleware? next = null)
        {
            this.next = next;
        }

        public bool Encode(byte input, out byte? output)
        {
            var encoded = EncodeByte(input);

            if (next == null || !encoded.HasValue)
            {
                output = encoded;
                return encoded.HasValue;
            }

            var didEncode = next.Encode(encoded.Value, out var nextEncoded);
            output = nextEncoded;
            return didEncode;
        }

        public bool Flush(out byte? flushed)
        {
            var encoded = FlushByte();

            if (next == null)
            {
                flushed = encoded;
                return encoded.HasValue;
            }

            if (!encoded.HasValue)
            {
                var buffering = next.Flush(out var nextFlushed);
                flushed = nextFlushed;
                return buffering;
            }

            next.Encode(encoded.Value, out var nextEncoded);
            flushed = nextEncoded;

            /*
			 * If the next middleware buffered the flushed byte, then the client code
			 * needs to keep calling Flush
			 */
            return true;
        }

        /// <summary>
        /// Encodes a byte or returns null if no byte was encoded.
        /// </summary>
        protected abstract byte? EncodeByte(byte input);

        /// <summary>
        /// Encodes a buffered byte or returns null if no was encoded.
        /// </summary>
        protected abstract byte? FlushByte();
    }
}
