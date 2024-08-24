namespace Compression.Lib.Framework
{
    public interface IEncoderMiddleware
    {
        bool Encode(byte input, out byte? output);

        bool Flush(out byte? flushed);

        IEncoderMiddleware? Next { get; }
    }
}
