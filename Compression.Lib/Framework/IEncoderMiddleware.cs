namespace Compression.Lib.Framework
{
    public interface IEncoderMiddleware
    {
        /// <summary>
        /// Consume one byte, either outputting the encoded value or buffering it.
        /// </summary>
        bool Encode(byte input, out byte? output);

        /// <summary>
        /// Consume any buffered input and output encoded values.
        /// </summary>
        bool Flush(out byte? flushed);

        /// <summary>
        /// Consume buffered output. This is for encoders with variable size output.
        /// </summary>
        bool FlushOutput(out byte? flushed);

		/// <summary>
		/// Check if any output is buffered. This is for encoders with variable size output.
		/// </summary>
		bool HasPendingOutput();

        IEncoderMiddleware? Next { get; }
    }
}
