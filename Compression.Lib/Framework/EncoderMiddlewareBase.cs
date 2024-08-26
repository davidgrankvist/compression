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

			var didEncode = Next.Encode(encoded.Value, out var nextEncoded);
			output = nextEncoded;
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
				var buffering = Next.Flush(out var nextFlushed);
				flushed = nextFlushed;
				return buffering;
			}

			Next.Encode(encoded.Value, out var nextEncoded);
			flushed = nextEncoded;

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

		/// <summary>
		///  Returns a buffered output byte or null if none exists.
		///  This is for variable size output.
		/// </summary>
		protected virtual byte? FlushOutputByte()
		{
			// assume single byte output by default
			return null;
		}

		/// <summary>
		/// Check if any output is buffered. This is for encoders with variable size output.
		/// </summary>
		public virtual bool HasPendingOutput()
		{
			// assume single byte output by default
			return false;
		}

		public bool FlushOutput(out byte? flushed)
		{
			var encoded = FlushOutputByte();

			if (Next == null)
			{
				flushed = encoded;
				return HasPendingOutput();
			}

			if (!encoded.HasValue)
			{
				Next.FlushOutput(out var nextFlushed);
				flushed = nextFlushed;
				return Next.HasPendingOutput();
			}

			Next.Encode(encoded.Value, out var nextEncoded);
			flushed = nextEncoded;
			return Next.HasPendingOutput();
		}
	}
}
