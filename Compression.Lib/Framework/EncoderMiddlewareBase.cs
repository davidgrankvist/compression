namespace Compression.Lib.Framework
{
    public abstract class EncoderMiddlewareBase : IEncoderMiddleware
    {
        public IEncoderMiddleware? Next { get; internal set; }

        protected EncoderMiddlewareBase(IEncoderMiddleware? next = null)
        {
            Next = next;
        }

        public bool Encode(byte input, out byte? output)
        {
            var encoded = EncodeByte(input);

            if (Next == null || !encoded.HasValue)
            {
                output = encoded;
                return encoded.HasValue;
            }

            var didEncode = Next.Encode(encoded.Value, out var NextEncoded);
            output = NextEncoded;
            return didEncode;
        }

        public bool Flush(out byte? flushed)
        {
            var encoded = FlushByte();

            if (Next == null)
            {
                flushed = encoded;
                return encoded.HasValue;
            }

            if (!encoded.HasValue)
            {
                var buffering = Next.Flush(out var NextFlushed);
                flushed = NextFlushed;
                return buffering;
            }

            Next.Encode(encoded.Value, out var NextEncoded);
            flushed = NextEncoded;

            /*
			 * If the Next middleware buffered the flushed byte, then the client code
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
